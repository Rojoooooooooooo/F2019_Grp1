using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetParadise.Models.Body
{
    public class CommentModel
    {
        public string Id { get; set; }
        public string ProfileId { get; set; }
        public string PostId { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}