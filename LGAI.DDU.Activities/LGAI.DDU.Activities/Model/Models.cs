using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LGAI.DDU.Activities.Model
{
    [JsonObject("molecule")]
    public class Molecule
    {
        [JsonProperty("bbox")]
        public int[] BBox { get; set; }

        [JsonProperty("layout_preserving_mol")]
        public string LayoutPreservingMol { get; set; }

        [JsonProperty("pred_mol")]
        public string PredMol { get; set; }

        [JsonProperty("smiles")]
        public string Smiles { get; set; }

        //RDkit을 이용해서 이미지로 변경해서 처리하는게 필요 이미지 파일로 줘야 할지? 
        //public System.Drawing.Imaging.Image PngPicture { get; set; }
        public string SvgImagePath { get; set; }

        public int Page { get; set; }

        public int Seq { get; set; }
    }

    [JsonObject("table")]
    public class Table
    {
        [JsonProperty("bbox")]
        public int[] BBox { get; set; }
        [JsonProperty("html")]
        public string HtmlBody { get; set; }
        [JsonProperty("idx")]
        public int Index { get; set; }   
    }

    [JsonObject("chart")]
    public class Chart
    {
        [JsonProperty("bbox")]
        public int[] BBox { get; set; }

        [JsonProperty("idx")]
        public int Index { get; set; }
        [JsonProperty("legend")]
        public string[] Legends { get; set; }


        [JsonProperty("chart_type")]
        public string ChartType { get; set; }

        [JsonProperty("title")]
        public string[] Title { get; set; }

        [JsonProperty("x_title")]
        public string[] XTitle { get; set; }

        [JsonProperty("y_title")]   
        public string[] YTitle { get; set; }

        [JsonProperty("x_labels")]
        public string[] XLabels { get; set; }

        [JsonProperty("y_labels")]
        public string[] YLabels { get; set; }
    }   

    [JsonObject("layout")]    
    public class Layout
    {
        [JsonProperty("box_label")]
        public string BoxLabel { get; set; }
        [JsonProperty("boxes")]
        public double[] Boxes { get; set; }
        [JsonProperty("norm_boxes")]
        public double[] NormalizedBoxes { get; set; }
        [JsonProperty("score")]
        public double score { get; set; }   
    }
    [JsonObject("elements")]
    public class Element
    {
        [JsonProperty("table")]
        public Table[] Tables { get; set; }

        [JsonProperty("chart")]
        public Chart[] Charts { get; set; }

        public bool ShouldSerializeTables()
        {
            return Tables != null && Tables.Length > 0;
        }
        public bool ShouldSerializeCharts()
        {
            return Charts != null && Charts.Length > 0;
        }
    }

    [JsonObject("outputs")]
    public class Output
    {
        [JsonProperty("elements")]
        public Element Element { get; set; }

        [JsonProperty("image_size")]
        public int [] ImageSize { get; set; }

        [JsonProperty("layout")]
        public Layout[] Layouts { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }
    }

    public class DDUResult
    {
        [JsonProperty("param")]
        public JObject param { get; set; }

        [JsonProperty("outputs")]
        public Output[] Outputs { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class ResponseOutput
    {
        [JsonProperty("estimated_time")]
        public int EstimatedTime { get; set; }
    }
    public class DDUResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("outputs")]
        public ResponseOutput[] outputs { get; set; }

        [JsonProperty("params")]
        public string Params { get; set; }
    }


    public class LGAIDDUResponse
    {
        public HttpStatusCode status { get; set; }
        public string body { get; set; }
    }

    public enum OnOff
    {
        On,
        Off
    }
}
