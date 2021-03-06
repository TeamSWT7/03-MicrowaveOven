﻿using NUnit.Framework;
using System;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IT8_TimeButton_UserInterface
    {
        //Uut
        private IButton _buttonTimer;
        private UserInterface _userInterface;

        //Output
        private IOutput _output;

        //Fakes for UserInterface Paramaters
        private IDoor _door;
        private IButton _buttonPower;
        private IButton _buttonStartCancel;
        private ILight _light;
        private IDisplay _display;
        private ICookController _cookController;

        [SetUp]
        public void SetUp()
        {
            #region Substitutes for paramaters for UserInterface

            _buttonStartCancel = Substitute.For<IButton>();
            _buttonPower = Substitute.For<IButton>();
            _light = Substitute.For<ILight>();
            _display = Substitute.For<IDisplay>();
            _cookController = Substitute.For<ICookController>();
            _door = Substitute.For<IDoor>();

            #endregion
            
            //Used for test
            _output = Substitute.For<IOutput>();
            _buttonTimer = new Button();
            _display = new Display(_output);
            _userInterface = new UserInterface(_buttonPower, _buttonTimer, _buttonStartCancel, _door, _display, _light, _cookController);
        }

        #region TimeButton/UI integrationtest

        [Test]
        public void TimeButtonDisplay_PressedButton_ButtonPressedOutputIsCorrect()
        {
            _userInterface.OnPowerPressed(null, EventArgs.Empty);
            _buttonTimer.Press();

            _output.Received(1).OutputLine("Display shows: 01:00");
        }

        [Test]
        public void TimeButtonDisplayTwoTimes_PressedButton_ButtonPressedOutputIsCorrect()
        {
            _userInterface.OnPowerPressed(null, EventArgs.Empty);
            _buttonTimer.Press();
            _buttonTimer.Press();

            _output.Received(1).OutputLine("Display shows: 01:00");
            _output.Received(1).OutputLine("Display shows: 02:00");
        }

        [Test]
        public void TimeButtonDisplayThreeTimes_PressedButton_ButtonPressedOutputIsCorrect()
        {
            _userInterface.OnPowerPressed(null, EventArgs.Empty);
            _buttonTimer.Press();
            _buttonTimer.Press();
            _buttonTimer.Press();

            _output.Received(1).OutputLine("Display shows: 03:00");
        }
        #endregion
    }
}
