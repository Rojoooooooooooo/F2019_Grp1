using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PetParadise.Extras
{
    public class UID
    {
        private byte Size;
        public UID(IdSize size)
        {
            Size = (byte)size;
        }
        public async Task<string> GenerateIdAsync()
        {

            string s = await Task.Run(() =>
            {
                Random random = new Random();
                byte[] buffer = new byte[(byte)this.Size / 2];
                random.NextBytes(buffer);
                String result = String.Concat(buffer.Select(b => b.ToString("x2")));



                return (byte)this.Size % 2 != 0 ?
                    result + random.Next(16).ToString("x") :
                    result;
            });

            return s;

        }

    }

    public enum IdSize
    {
        SHORT = 8,
        LONG = 16
    }
}