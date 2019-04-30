using NUnit.Framework;
using System;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IT5_Door_UserInterface
    {
        //Uut
        private Door _door;
        private UserInterface _userInterface;

        //Output
        private IOutput _output;

        //Fakes for UserInterface Paramaters
        private IButton _buttonPower;
        private IButton _buttonTimer;
        private IButton _buttonStartCancel;
        private ILight _light;
        private IDisplay _display;
        private ICookController _cookController;

        [SetUp]
        public void SetUp()
        {
            #region Substitutes for paramaters for UserInterface
            _buttonPower = Substitute.For<IButton>();
            _buttonStartCancel = Substitute.For<IButton>();
            _buttonTimer = Substitute.For<IButton>();
            _light = Substitute.For<ILight>();
            _display = Substitute.For<IDisplay>();
            _cookController = Substitute.For<ICookController>();

            #endregion
            
            //Used for test
            _output = Substitute.For<IOutput>();
            _door = new Door();
            _light = new Light(_output);
            _userInterface = new UserInterface(_buttonPower, _buttonTimer, _buttonStartCancel, _door, _display, _light, _cookController);
            
        }

        #region Door/UI integrationtest
        
        [Test]
        public void DoorOpen_DoorOpnes_DoorIsOpen()
        {
            _door.Open();

           _userInterface.OnStartCancelPressed(null, EventArgs.Empty);

           _output.Received(1).OutputLine("Light is turned on");
        }

        [Test]
        public void DoorClose_DoorIsOpenAndThenCloses_DoorIsClosed()
        {
            // Open door
            _door.Open();
            _output.Received(1).OutputLine("Light is turned on");
        
            // Close door
            _door.Close();
            _userInterface.OnStartCancelPressed(null, EventArgs.Empty);

            _output.Received(1).OutputLine("Light is turned off");
        }

        [Test]
        public void DoorCloses_OpensDoorClosesDoorOpensDoor_DoorIsOpen()
        {
            _door.Open();
            _door.Close();
            _door.Open();

            _output.Received(2).OutputLine("Light is turned on");
            _output.Received(1).OutputLine("Light is turned off");
        }

        #endregion
    }
}
