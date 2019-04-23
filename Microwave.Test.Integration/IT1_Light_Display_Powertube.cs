using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IT1_Light_Display_Powertube
    {
        private Light _light;
        private Display _display;
        private PowerTube _powerTube;
        private IOutput _output;

        [SetUp]
        public void SetUp()
        {
            _output = Substitute.For<IOutput>();
            _light = new Light(_output);
            _display = new Display(_output);
            _powerTube = new PowerTube(_output);
        }

        #region Light integrationtest

        [Test]
        public void Test()
        {

        }

        #endregion
    }
}
