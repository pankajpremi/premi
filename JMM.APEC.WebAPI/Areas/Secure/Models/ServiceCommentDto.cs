using System;
using System.Collections.Generic;

namespace JMM.APEC.WebAPI.Models
{
    public class ServiceCommentDto
    {
        public int CommentId { get; set; }

        public int EntityId { get; set; }

        public string Comments { get; set; }

        public int EnteredByUserId { get; set; }

        public DateTime CommentDate { get; set; }

        public IList<ServiceCommentDto> ChildComments { get; set; }

        public int? MediaId;        

        public int ObjectId;
        

    }
}