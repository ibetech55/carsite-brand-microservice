using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrandMicroservice.src.Utils;

namespace BrandMicroservice.Test.Utils
{
    public class GenerateCodeTest
    {
        [Fact]
        public void ExecuteWithSize8ReturnString()
        {

            var res = GenerateCode.Execute(8);

            Assert.Equal(8, res.Length);
        }
    }
}
