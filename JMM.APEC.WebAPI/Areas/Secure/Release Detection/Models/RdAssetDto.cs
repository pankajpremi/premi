using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMM.APEC.WebAPI.Areas.Secure.Models
{
    public class RdAssetDto
    {
        public int AssetTypeId { get; set; }
        public string AssetTypeName { get; set; }
        public int AssetId { get; set; }
        public string AssetLabel { get; set; }
        public string AssetProperty1Label { get; set; }
        public string AssetProperty1Value { get; set; }
        public string AssetProperty2Label { get; set; }
        public string AssetProperty2Value { get; set; }
    }
}