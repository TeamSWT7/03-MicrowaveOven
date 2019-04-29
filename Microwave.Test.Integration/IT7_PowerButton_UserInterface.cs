using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NSubstitute.ReceivedExtensions;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IT7_PowerButton_UserInterface
    {
        //Uut
        private IButton _buttonPower;
        private UserInterface _userInterface;

        //Output
        private IOutput _output;

        //Fakes for UserInterface Paramaters
        private IDoor _door;
        private IButton _buttonTimer;
        private IButton _buttonStartCancel;
        private ILight _light;
        private IDisplay _display;
        private ICookController _cookController;




        [SetUp]
        public void SetUp()
        {
            #region Substitutes for paramaters for UserInterface
            _buttonStartCancel = Substitute.For<IButton>();
            _buttonTimer = Substitute.For<IButton>();
            _light = Substitute.For<ILight>();
            _display = Substitute.For<IDisplay>();
            _cookController = Substitute.For<ICookController>();
            _door = Substitute.For<IDoor>();

            #endregion
            
            //Used for test
            _output = Substitute.For<IOutput>();
            _buttonPower = new Button();
            _display = new Display(_output);
            _userInterface = new UserInterface(_buttonPower, _buttonTimer, _buttonStartCancel, _door, _display, _light, _cookController);
            
        }

        #region PowerButton/UI integrationtest
        
        [Test]
        public void PowerButtonDisplay_PressedButton_ButtonPressedDisplayShows50W()
        {
            _buttonPower.Press();

            //50 W is default
            _output.Received(1).OutputLine("Display shows: 50 W");
        }

        [Test]
        public void PowerButtonDisplay_PressedButton_ButtonPressedDisplayShows100W()
        {
            _buttonPower.Press();
            _buttonPower.Press();

            _output.Received(1).OutputLine("Display shows: 100 W");
        }

        [Test]
        public void PowerButtonDisplays_MicrowaveIsOff_OutputIsCorrect()
        {
            for (int i = 50; i <= 700; i += 50)
            {
                _buttonPower.Press();
                _buttonPower.Press();
            }

            _buttonPower.Press();
            

            _output.Received(3).OutputLine("Display shows: 50 W");
        }

        #endregion
    }
}
