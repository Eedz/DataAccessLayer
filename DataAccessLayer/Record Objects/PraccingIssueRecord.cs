using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ITCLib
{
    public class PraccingIssueRecord : IRecord<PraccingIssue>
    {
        public bool NewRecord { get ; set ; }
        public bool Dirty { get; set; }

        public PraccingIssue Item { get; set; }

        public List<PraccingResponse> AddedResponses { get; set; }
        public List<PraccingResponse> EditedResponses { get; set; }
        public List<PraccingResponse> DeletedResponses { get; set; }

        public List<PraccingImage> AddedImages { get; set; }
        public List<PraccingImage> DeletedImages { get; set; }

        public List<PraccingImage> AddedResponseImages { get; set; }
        public List<PraccingImage> DeletedResponseImages { get; set; }

        public PraccingIssueRecord()
        {
            Item = new PraccingIssue();

            AddedResponses = new List<PraccingResponse>();
            EditedResponses = new List<PraccingResponse>();
            DeletedResponses = new List<PraccingResponse>();

            AddedImages = new List<PraccingImage>();
            DeletedImages = new List<PraccingImage>();

            AddedResponseImages = new List<PraccingImage>();
            DeletedResponseImages = new List<PraccingImage>();
        }

        public PraccingIssueRecord(PraccingIssue item)
        {
            Item = item;

            AddedResponses = new List<PraccingResponse>();
            EditedResponses = new List<PraccingResponse>();
            DeletedResponses = new List<PraccingResponse>();

            AddedImages = new List<PraccingImage>();
            DeletedImages = new List<PraccingImage>();

            AddedResponseImages = new List<PraccingImage>();
            DeletedResponseImages = new List<PraccingImage>();
        }

        public int SaveRecord()
        {
            if (NewRecord)
            {
                if (DBAction.InsertPraccingIssue(Item) == 1)
                    return 1;

                NewRecord = false;
                Dirty = false;
            }
            else if (Dirty)
            {
                if (DBAction.UpdatePraccingIssue(Item) == 1)
                    return 1;

                Dirty = false;
            }

            SaveResponses();

            SaveImages();

            return 0;
        }

        public int SaveResponses()
        {
            foreach(PraccingResponse response in AddedResponses)
            {
                response.IssueID = Item.ID;
                DBAction.InsertPraccingResponse(response);
            }
            AddedResponses.Clear();

            foreach (PraccingResponse response in EditedResponses)
            {
                DBAction.UpdatePraccingResponse(response);
            }
            EditedResponses.Clear();

            foreach (PraccingResponse response in DeletedResponses)
            {
                DBAction.DeleteRecord(response);
            }
            DeletedResponses.Clear();

            return 0;
        }

        public int SaveImages()
        {
            foreach (PraccingImage img in AddedImages)
            {
                img.PraccID = Item.ID;
                DBAction.InsertPraccingImage(img);
            }
            AddedImages.Clear();

            foreach (PraccingImage img in DeletedImages)
            {
                DBAction.DeleteRecord(img);
                try
                {
                    File.Delete(img.Path);
                }
                catch
                {

                }
            }
            DeletedImages.Clear();

            foreach (PraccingImage img in AddedResponseImages)
            {
                var response = Item.Responses.Where(x => x.Images.Contains(img)).FirstOrDefault();
                if (response == null) continue;
                img.PraccID = response.ID;
                DBAction.InsertPraccingResponseImage(img);
            }
            AddedResponseImages.Clear();

            foreach (PraccingImage img in DeletedResponseImages)
            {
                DBAction.DeletePraccResponseImage(img);
                try
                {
                    File.Delete(img.Path);
                }
                catch
                {

                }
            }
            DeletedResponseImages.Clear();

            return 0;
        }
    }
}
