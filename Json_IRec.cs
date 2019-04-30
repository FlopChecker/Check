using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TonicDS.BasicAmbients;

namespace AlfaRomeo_UWP.jsWebView
{
    public class Json_IRec : TonicDS.BasicAmbients.IRecHierarch
    {
        public Newtonsoft.Json.Linq.JObject thisJObject;
        public Newtonsoft.Json.Linq.JArray thisJArray;
        public Newtonsoft.Json.Linq.JToken thisJToken;

        public string thisString;
        public List<Json_IRec> thisSubs = new List<Json_IRec>();

        public static Json_IRec FromResult(string result) {
            var t = JToken.Parse(result);
            var n = new Json_IRec(t);
            return n;
            //thisJArray = Newtonsoft.Json.Linq.JArray.Parse(result);
            //var t = Newtonsoft.Json.Linq.JToken.Parse(result);
            ////thisJObject = Newtonsoft.Json.Linq.JObject.Parse(result);
            ////thisString = thisJObject.
            //thisString = thisJArray.Path;
            //foreach (var j in thisJArray) {
            //    thisSubs.Add(new Json_IRec(j));
            //}
        }
        public Json_IRec(Newtonsoft.Json.Linq.JToken jt) {
            thisJToken = jt;
            if (jt is JArray) {
                thisString = "[Array]";
                foreach (var j in thisJToken.Children())
                    thisSubs.Add(new Json_IRec(j));
            }
            else if (jt is JObject) {
                thisString = "[Object]";
                foreach (var j in thisJToken.Children())
                    thisSubs.Add(new Json_IRec(j));
            }
            else if (jt is JProperty) {
                var jp = jt as JProperty;
                var val = jp.Value;
                if (jp.Value is JObject) val = "[Object]";
                if (jp.Value is JArray) val = "[Array]";
                thisString = jp.Name + ": " + val;

                if (jp.Value is JObject | jp.Value is JArray)
                    foreach (var j in jp.Value.Children())
                        thisSubs.Add(new Json_IRec(j));
            }

            else {
                thisString = jt.Path;
                if (jt is Newtonsoft.Json.Linq.JValue) thisString = (jt as Newtonsoft.Json.Linq.JValue).Value.ToString();
                thisJToken = jt;
                if (jt.HasValues) {
                    foreach (var j in thisJToken.Children())
                        thisSubs.Add(new Json_IRec(j));
                }
            }

        }



        public bool iRec_HasSubs(IRec_Promise promiseCb = null) {
            return thisSubs.Any();
        }

        public string iRec_StringContent(IRec_Promise promiseCb = null) {
            return thisString;
        }

        public IEnumerable<IRecHierarch> iRec_SubsH(IRec_Promise promiseCb = null) {
            return thisSubs;
        }
    }
}
