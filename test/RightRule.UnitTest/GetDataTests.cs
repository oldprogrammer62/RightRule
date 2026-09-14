using BeSoft;
using System;
using System.Collections.Generic;
using System.Text;

namespace RightRuleTests
{
    public class GetDataTests
    {
        Dictionary<string, object> dictdata = new Dictionary<string, object>()
        {
            { "Value1", 100 },
            { "Value2", 200 }
        };

        int a = 300;
        int b = 400;

        (string Name, int Number) datatuple = (Name: "Value", Number: 100);

        [Fact]
        public void GetDataSets()
        {
            IRightRule rr = new BeSoft.RightRule();

            var regData1 = rr.RegisterData("Data1", dictdata);

            //var xxxx = rr.GetRegisteredDatasets();
            //Assert.NotNull(xxxx);
        }

        [Fact]
        public void GetDataObjects()
        {
            IRightRule rr = new BeSoft.RightRule();

            var regData1 = rr.RegisterData("A", nameof(a), a);
            var regData2 = rr.RegisterData("B", nameof(b), b);

            //var xxxx = rr.GetRegisteredDataObjects();
            //Assert.NotNull(xxxx);
        }

        [Fact]
        public void GetDatas()
        {
            IRightRule rr = new BeSoft.RightRule();

            var regData1 = rr.RegisterData("Data1", dictdata);

            var regData2 = rr.RegisterData("A", nameof(a), a);
            var regData3 = rr.RegisterData("B", nameof(b), b);

            var regData4 = rr.RegisterData("C", datatuple);

            var xxxx = rr.newGetRegisteredData();
            Assert.NotNull(xxxx);
        }

        [Fact]
        public void ResolveVariables()
        {
            IRightRule rr = new BeSoft.RightRule();

            var regData1 = rr.RegisterData("Data1", dictdata);

            var regData2 = rr.RegisterData("A", nameof(a), a);
            var regData3 = rr.RegisterData("B", nameof(b), b);

            var regData4 = rr.RegisterData("C", datatuple);

            var xxxx = rr.newGetRegisteredData();
            Assert.NotNull(xxxx);

            var rule1def = rr.BuildRule()
                .Number(1)
                .Name("Name1")
                .Context("Context")
                .Statement("A = B")
                .Decision("TRUE")
                .FalseDecision("FALSE")
                .Build();

            var rule1reg = rr.RegisterRule(rule1def);
            var result = rr.ApplyRules("Context");
            Assert.NotNull(result);
        }
    }
}
