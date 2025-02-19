using Breath_of_the_Wild_Multiplayer.MVVM.Model.DTO;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Breath_of_the_Wild_Multiplayer.Source_files
{
    public class JSONCompressor
    {
        List<byte> ByteData;

        public byte[] BuildArrayOfBytes(object original, bool debug = false)
        {
            ByteData = new List<byte>();

            GetByteData(original);

            if (!debug)
                ByteData.InsertRange(0, BitConverter.GetBytes((short)ByteData.Count));

            return ByteData.ToArray();
        }

        private void GetByteData(object original)
        {
            if (original.GetType() == typeof(int))
                AddBytes(BitConverter.GetBytes((int)original));
            else if (original.GetType() == typeof(float))
                AddBytes(BitConverter.GetBytes((float)original));
            else if (original.GetType() == typeof(bool))
                ByteData.Add((bool)original ? (byte)1 : (byte)0);
            else if (original.GetType() == typeof(byte))
                ByteData.Add((byte)original);
            else if (original.GetType() == typeof(short))
                AddBytes(BitConverter.GetBytes((short)original));
            else if (original.GetType() == typeof(string))
            {
                ByteData.Add((byte)((string)original).Length);
                AddBytes(Encoding.UTF8.GetBytes((string)original), false);
            }
            else if (original.GetType() == typeof(Vec3f))
            {
                Vec3f originalVec3f = (Vec3f)original;

                AddBytes(BitConverter.GetBytes(originalVec3f.x));
                AddBytes(BitConverter.GetBytes(originalVec3f.y));
                AddBytes(BitConverter.GetBytes(originalVec3f.z));
            }
            else if (original.GetType().IsGenericType && typeof(System.Collections.IList).IsAssignableFrom(original.GetType()))
            {
                typeof(JSONCompressor).GetMethod("AddListData").MakeGenericMethod(original.GetType().GenericTypeArguments).Invoke(this, new[] { original });
            }
            else if (original.GetType().IsGenericType && typeof(System.Collections.IDictionary).IsAssignableFrom(original.GetType()))
            {
                typeof(JSONCompressor).GetMethod("AddDictData").MakeGenericMethod(original.GetType().GenericTypeArguments).Invoke(this, new[] { original });
            }
            else
            {
                foreach (var item in original.GetType().GetFields())
                    GetByteData(item.GetValue(original));
            }
        }

        public void AddListData<T>(object original)
        {
            List<T> OriginalList = (List<T>)original;

            ByteData.Add((byte)OriginalList.Count);

            for (int i = 0; i < OriginalList.Count; i++)
            {
                GetByteData(OriginalList[i]);
            }
        }

        public void AddDictData<K, V>(object original)
        {
            Dictionary<K, V> OriginalDict = (Dictionary<K, V>)original;

            ByteData.Add((byte)OriginalDict.Count);

            for (int i = 0; i < OriginalDict.Count; i++)
            {
                GetByteData(OriginalDict.ElementAt(i).Key);
                GetByteData(OriginalDict.ElementAt(i).Value);
            }
        }

        private void AddBytes(byte[] bytes, bool Reverse = true)
        {
            ByteData.AddRange(bytes);
        }
    }
}
