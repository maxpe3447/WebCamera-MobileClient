using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WebCameraClient.Model;

namespace WebCameraClient.Helpers
{
    public static class ActionDataModelConvert
    {
        public static byte[] ToBytes(ActionDataModel actionDataModel)
        {
            var jsonData = JsonConvert.SerializeObject(actionDataModel);

            byte[] jsonDataBytes = Encoding.ASCII.GetBytes(jsonData);

            return jsonDataBytes;
        }

        public static ActionDataModel ToModel(byte[] bytes)
        {
            var str = Encoding.ASCII.GetString(bytes, 0, bytes.Length);

            var actionData = JsonConvert.DeserializeObject<ActionDataModel>(str) ?? new ActionDataModel();

            return actionData;
        }
    }
}
