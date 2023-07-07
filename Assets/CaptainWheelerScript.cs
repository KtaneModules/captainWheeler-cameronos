using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;
using Rnd = UnityEngine.Random;
using Math = ExMath;
using RealMath = System.Math;

public class CaptainWheelerScript : MonoBehaviour {

   public KMBombInfo Bomb;
   public KMBombInfo BombInfo;
   public KMAudio Audio;
   public RotateWheel wheel;
   public RotateDisplays hat;
   public KMSelectable[] Buttons;
   public TextMesh[] DisplayTexts;
   public Renderer Wheel;
   public Material[] WheelColors;
   public Renderer HatBadge;
   public Sprite[] HatBadges;

   static int ModuleIdCounter = 1;
   int ModuleId;
   private bool ModuleSolved;
   private bool wheelDown = false;
   private const float _interactionPunchIntensity = .5f;
   private string[] captainNames;
   private string selectedName;
   private int currentSailorIndex = 0;
   private int[] randomSpeeds = {1, 2, 3};
   private int[] randomDirections = {1, 2};
   private int[] randomHatBadges = {1, 2, 3};
   private int randomHatBadge;
   private int randomColor;
   private int chosenDirection;

   private string[] shipsReadingOrder;
   private string[] shipsBrailleOrder;
   private string[] shipsChineseOrder;
   private string BoatName;

   private int HatNumber;

   //correct items
   private string correctCaptain;
   private int correctCaptainIndex;
   private string correctShip;
   private int chosenSpeed;
   private int tableSpeed;

   private string[] shipNames;
   private string selectedShip;
   private int currentShipIndex = 0;

   void Awake () {
      ModuleId = ModuleIdCounter++;
      GetComponent<KMBombModule>().OnActivate += Activate;
      /*
      foreach (KMSelectable object in keypad) {
          object.OnInteract += delegate () { keypadPress(object); return false; };
      }
      */

      Buttons[0].OnInteract += delegate () { buttonPress(1); return false; }; //wheel
      Buttons[1].OnInteract += delegate () { buttonPress(2); return false; }; //captain left
      Buttons[2].OnInteract += delegate () { buttonPress(3); return false; }; //captain right
      Buttons[3].OnInteract += delegate () { buttonPress(4); return false; }; //ship left
      Buttons[4].OnInteract += delegate () { buttonPress(5); return false; }; //ship right
      Buttons[5].OnInteract += delegate () { buttonPress(6); return false; }; //submit

      Buttons[0].AddInteractionPunch(_interactionPunchIntensity);
      Buttons[1].AddInteractionPunch(_interactionPunchIntensity);
      Buttons[2].AddInteractionPunch(_interactionPunchIntensity);
      Buttons[3].AddInteractionPunch(_interactionPunchIntensity);
      Buttons[4].AddInteractionPunch(_interactionPunchIntensity);
      Buttons[5].AddInteractionPunch(_interactionPunchIntensity);
   }

   void buttonPress(int button) {
     if(!ModuleSolved){
     switch(button)
     {
       case 1:
       if(!wheelDown)
       {
       Audio.PlaySoundAtTransform("WheelPress", Buttons[0].transform);
       wheel.GoDown();
       hat.GoUp();
       wheelDown = true;
     }
       break;

       case 2:
       if(wheelDown){
       Audio.PlaySoundAtTransform("ButtonPress", Buttons[0].transform);
       currentSailorIndex = (currentSailorIndex - 1 + captainNames.Length) % captainNames.Length;
       selectedName = captainNames[currentSailorIndex];
       ChangeFontSize();
       DisplayTexts[0].text = selectedName.ToUpper();
     }
       break;

       case 3:
       if(wheelDown){
       Audio.PlaySoundAtTransform("ButtonPress", Buttons[0].transform);
       currentSailorIndex = (currentSailorIndex + 1) % captainNames.Length;
       selectedName = captainNames[currentSailorIndex];
       ChangeFontSize();
       DisplayTexts[0].text = selectedName.ToUpper();
     }
       break;

       case 4:
       if(wheelDown){
       Audio.PlaySoundAtTransform("ButtonPress", Buttons[0].transform);
       currentShipIndex = (currentShipIndex - 1 + shipNames.Length) % shipNames.Length;
       selectedShip = shipNames[currentShipIndex];
       ChangeFontSize();
       DisplayTexts[1].text = selectedShip.ToUpper();
     }
       break;

       case 5:
       if(wheelDown){
       Audio.PlaySoundAtTransform("ButtonPress", Buttons[0].transform);
       currentShipIndex = (currentShipIndex + 1) % shipNames.Length;
       selectedShip = shipNames[currentShipIndex];
       ChangeFontSize();
       DisplayTexts[1].text = selectedShip.ToUpper();
     }
       break;

       case 6:
       if(wheelDown){
       Audio.PlaySoundAtTransform("ButtonPress", Buttons[0].transform);
       string displayedSailor = DisplayTexts[0].text.ToUpper();
       string displayedShip = DisplayTexts[1].text.ToUpper();
       Debug.LogFormat("[Captain Wheeler " + "#" + ModuleId + "] The submitted sailor was: " + displayedSailor + ", and the submitted ship was: " + displayedShip + ".", ModuleId);
       float Clock = Bomb.GetTime();
       if(displayedSailor == correctCaptain.ToUpper() && displayedShip == correctShip.ToUpper())
       {
         DetermineCorrectTime(Clock);
       }
       else
       {
         Debug.LogFormat("[Captain Wheeler " + "#" + ModuleId + "] Strike! The wrong sailor and/or ship was inputted.", ModuleId);
         Strike();
         Audio.PlaySoundAtTransform("Strike", Buttons[0].transform);
       }
     }
       break;
     }
   }
   else
   {
     //Nothing. Module is solved
   }
 }

   void OnDestroy () { //Shit you need to do when the bomb ends

   }

   void Activate () { //Shit that should happen when the bomb arrives (factory)/Lights turn on

   }

  void RandomChoices()
    {
        //Random properties
        int randomInt = UnityEngine.Random.Range(0, captainNames.Length);
        string randomName = captainNames[randomInt];
        int randomSailorIndex = randomInt;
        currentSailorIndex = randomSailorIndex;
        ChangeFontSize();
        DisplayTexts[0].text = randomName.ToUpper();
        //ship stuff
        int randomInt2 = UnityEngine.Random.Range(0, shipNames.Length);
        string randomShip = shipNames[randomInt2];
        int randomShipIndex = randomInt2;
        currentShipIndex = randomShipIndex;
        ChangeFontSize();
        DisplayTexts[1].text = randomShip.ToUpper();

        //Random color for wheel
        randomColor = UnityEngine.Random.Range(0, WheelColors.Length);
        wheel.GetComponent<Renderer>().sharedMaterial = WheelColors[randomColor];
        switch(randomColor)
        {
          case 0:
          Debug.LogFormat("[Captain Wheeler " + "#" + ModuleId + "] The wheel is red.", ModuleId);
          break;

          case 1:
          Debug.LogFormat("[Captain Wheeler " + "#" + ModuleId + "] The wheel is green.", ModuleId);
          break;

          case 2:
          Debug.LogFormat("[Captain Wheeler " + "#" + ModuleId + "] The wheel is blue.", ModuleId);
          break;

          case 3:
          Debug.LogFormat("[Captain Wheeler " + "#" + ModuleId + "] The wheel is yellow.", ModuleId);
          break;

          case 4:
          Debug.LogFormat("[Captain Wheeler " + "#" + ModuleId + "] The wheel is magenta.", ModuleId);
          break;
        }

        //Random badge for hat
        randomHatBadge = UnityEngine.Random.Range(0, HatBadges.Length);
        HatBadge.GetComponent<SpriteRenderer>().sprite = HatBadges[randomHatBadge];

        //Random direction
        chosenDirection = UnityEngine.Random.Range(0, randomDirections.Length);
        wheel.ChangeDirection(chosenDirection);
        switch(chosenDirection)
        {
        case 0:
        Debug.LogFormat("[Captain Wheeler " + "#" + ModuleId + "] The wheel is rotating counter-clockwise.", ModuleId);
        break;

        case 1:
        Debug.LogFormat("[Captain Wheeler " + "#" + ModuleId + "] The wheel is rotating clockwise.", ModuleId);
        break;
      }

        //Random rotation speed
        int chosenSpeed = UnityEngine.Random.Range(0, randomSpeeds.Length);
        switch(chosenSpeed)
        {
          case 0:
          wheel.ChangeSpeed(0);
          tableSpeed = 0;
          Debug.LogFormat("[Captain Wheeler " + "#" + ModuleId + "] The wheel is rotating slowly.", ModuleId);
          break;

          case 1:
          wheel.ChangeSpeed(1);
          tableSpeed = 1;
          Debug.LogFormat("[Captain Wheeler " + "#" + ModuleId + "] The wheel is rotating moderately (medium).", ModuleId);
          break;

          case 2:
          wheel.ChangeSpeed(2);
          tableSpeed = 2;
          Debug.LogFormat("[Captain Wheeler " + "#" + ModuleId + "] The wheel is rotating fast.", ModuleId);
          break;
        }

        switch(randomHatBadge)
        {
          case 0:
            Debug.LogFormat("[Captain Wheeler " + "#" + ModuleId + "] The captain hat badge is gold.", ModuleId);
            HatNumber = 1;
            break;
          case 1:
            Debug.LogFormat("[Captain Wheeler " + "#" + ModuleId + "] The captain hat badge is black and white.", ModuleId);
            HatNumber = 2;
            break;
          case 2:
            Debug.LogFormat("[Captain Wheeler " + "#" + ModuleId + "] The captain hat badge is normal.", ModuleId);
            HatNumber = 3;
            break;
        }
    }

    void DetermineCorrectCaptain()
    {
      //determine
      bool hasEven = Bomb.GetSerialNumberNumbers().Last() % 2 == 0;
      if(hasEven)
      {
        switch(randomColor) // wheel color
        {
          case 0: //red
          if(chosenDirection == 0) //counter-clockwise
          {
            correctCaptain = "Davy Jones";
            correctCaptainIndex = 9;
          }
          else //clockwise
          {
            correctCaptain = "Cpt. Morgan";
            correctCaptainIndex = 4;
          }
          break;

          case 1: //green
          if(chosenDirection == 0) //counter-clockwise
          {
            correctCaptain = "Cpt. Squiggle";
            correctCaptainIndex = 2;
          }
          else //clockwise
          {
            correctCaptain = "Cpt. Jack";
            correctCaptainIndex = 8;
          }
          break;

          case 2: //blue
          if(chosenDirection == 0) //counter-clockwise
          {
            correctCaptain = "Adm. Flappa";
            correctCaptainIndex = 3;
          }
          else //clockwise
          {
            correctCaptain = "Adm. Bubbles";
            correctCaptainIndex = 1;
          }
          break;

          case 3://yellow
          if(chosenDirection == 0) //counter-clockwise
          {
            correctCaptain = "Adm. Quirky";
            correctCaptainIndex = 7;
          }
          else //clockwise
          {
            correctCaptain = "Cpt. Blackbeard";
            correctCaptainIndex = 6;
          }
          break;

          case 4: //magenta
          if(chosenDirection == 0) //counter-clockwise
          {
            correctCaptain = "Adm. Snicker";
            correctCaptainIndex = 5;
          }
          else //clockwise
          {
            correctCaptain = "Cpt. Wheeler";
            correctCaptainIndex = 0;
          }
          break;
        }
      }
      else
      {
        //not even
        switch(randomColor)
        {
          case 0: //red
          if(chosenDirection == 0) //counter-clockwise
          {
            correctCaptain = "Cpt. Squiggle";
            correctCaptainIndex = 2;
          }
          else //clockwise
          {
            correctCaptain = "Cpt. Blackbeard";
            correctCaptainIndex = 6;
          }
          break;

          case 1: //green
          if(chosenDirection == 0) //counter-clockwise
          {
            correctCaptain = "Cpt. Jack";
            correctCaptainIndex = 8;
          }
          else //clockwise
          {
            correctCaptain = "Cpt. Morgan";
            correctCaptainIndex = 4;
          }
          break;

          case 2: //blue
          if(chosenDirection == 0) //counter-clockwise
          {
            correctCaptain = "Cpt. Wheeler";
            correctCaptainIndex = 0;
          }
          else //clockwise
          {
            correctCaptain = "Davy Jones";
            correctCaptainIndex = 9;
          }
          break;

          case 3://yellow
          if(chosenDirection == 0) //counter-clockwise
          {
            correctCaptain = "Adm. Snicker";
            correctCaptainIndex = 5;
          }
          else //clockwise
          {
            correctCaptain = "Adm. Quirky";
            correctCaptainIndex = 7;
          }
          break;

          case 4: //magenta
          if(chosenDirection == 0) //counter-clockwise
          {
            correctCaptain = "Adm. Flappa";
            correctCaptainIndex = 3;
          }
          else //clockwise
          {
            correctCaptain = "Adm. Bubbles";
            correctCaptainIndex = 1;
          }
          break;
        }
      }
      Debug.LogFormat("[Captain Wheeler " + "#" + ModuleId + "] The correct captain is: " + correctCaptain + ".", ModuleId);
    }

    int FetchMeTheirSouls()
    {
      IEnumerable<char> serialNumber = Bomb.GetSerialNumberLetters();
      List<char> serialNumberList = serialNumber.ToList();
      char firstLetter = char.ToUpper(serialNumberList.First());
      int position = firstLetter - 'A' + 1;
      //Debug.Log("The first letter of the serial is: " + firstLetter + ", so position is: " + position + ".");
      return position;
    }

    void DetermineCorrectShip()
    {
      //Serial Number letter
      int letterPos = FetchMeTheirSouls();
      int lastDigitOfSerial = Bomb.GetSerialNumberNumbers().Last();
      //Debug.Log("Since correct captain is: " + correctCaptain + ", correct captain index is: " + correctCaptainIndex);
      int[,] array = new int[10,3]
          {
        {12,20,16},
        {11,5,25},
        {6,24,3},
        {19,9,4},
        {2,23,21},
        {18,13,27},
        {15,8,14},
        {10,28,17},
        {1,30,7},
        {22,26,29}
      };
      int tableReport = array[correctCaptainIndex, tableSpeed];
      //Debug.Log("Table reports: " + tableReport + ".");

      //letterPos, lastDigitOfSerial
      int totalOfTwo = letterPos + lastDigitOfSerial;
      int totalOfThree = RealMath.Abs(totalOfTwo - tableReport);
      int final = totalOfThree % 10;
      int indicOnCount = Bomb.GetOnIndicators().ToArray().Length;
      int indicOffCount = Bomb.GetOffIndicators().ToArray().Length;
      //Debug.Log("Amount of on indicators are: " + indicOnCount + ", and amount off are: " + indicOffCount + ".");

      if(indicOnCount > indicOffCount)
      {
        //Reading order
        BoatName = shipsReadingOrder[final];
      }
      else if(indicOffCount > indicOnCount)
      {
        //Braille
        BoatName = shipsBrailleOrder[final];
      }
      else if(indicOnCount == indicOffCount)
      {
        //Chinese
        BoatName = shipsChineseOrder[final];
      }
      correctShip = BoatName;
      Debug.LogFormat("[Captain Wheeler " + "#" + ModuleId + "] The correct ship is: " + correctShip + ".", ModuleId);
    }

    int CalculateDigitalRoot(IEnumerable<int> numbers)
        {
            int digitalRoot = numbers.Sum();

            while (digitalRoot >= 10)
            {
                digitalRoot = GetDigitSum(digitalRoot);
            }

            return digitalRoot;
        }

        int GetDigitSum(int number)
        {
            int sum = 0;

            while (number > 0)
            {
                sum += number % 10;
                number /= 10;
            }

            return sum;
        }

    void BeforeDetermineCorrectTime(){
      switch(HatNumber)
      {
        case 1: //gold
        int firstDigitOfSerial = Bomb.GetSerialNumberNumbers().First();
        int[] possibleTimers;

        if (firstDigitOfSerial >= 10)
                {
                    // If the first digit is greater than or equal to 10, add the first digit as a possible timer value
                    possibleTimers = new int[] { firstDigitOfSerial % 100 }; // Ensure the value is less than 100
                }
                else
                {
                    // If the first digit is less than 10, generate all possible combinations of the first digit and the last two seconds
                    possibleTimers = Enumerable.Range(0, 100)
                        .Where(t => (t / 10 + t % 10) == firstDigitOfSerial && t <= 59)
                        .ToArray();
                }

                string goldValues = string.Join(", ", possibleTimers.Select(t => t.ToString("D2")).ToArray());
                Debug.LogFormat("[Captain Wheeler #" + ModuleId + "] The possible times to submit are: " + goldValues + ".");
        break;

        case 2: //black and white
        string BWvalues = "11, 10, 01, 00";
        Debug.LogFormat("[Captain Wheeler " + "#" + ModuleId + "] The possible times to submit are: " + BWvalues + ".", ModuleId);
        break;

        case 3: //normal
        int[] possibleTimers2;
        int digitalRoot = CalculateDigitalRoot(Bomb.GetSerialNumberNumbers());
        if (digitalRoot >= 10)
        {
            // If the digital root is greater than or equal to 10, use the last digit as a possible timer value
            possibleTimers2 = new int[] { digitalRoot % 10 };
        }
        else
        {
            // If the digital root is less than 10, generate all possible combinations of the digital root and the last digit of the seconds timer
            possibleTimers2 = Enumerable.Range(0, 100)
                .Where(t => (t % 10) == digitalRoot && t <= 59)
                .ToArray();
        }

        string possibleTimersString = string.Join(", ", possibleTimers2.Select(t => t.ToString("D2")).ToArray());
        Debug.LogFormat("[Captain Wheeler #{0}] The possible times to submit are: {1}.", ModuleId, possibleTimersString);
        break;
      };
    }

    void DetermineCorrectTime(float Timer){
      //Time to time it up
      //BIRD UP!
      switch(HatNumber)
      {
        case 1: //gold
        int firstDigitOfSerial = Bomb.GetSerialNumberNumbers().First();
        int remainingSeconds = (int)Timer % 60;
        int lastSecondsDigit = remainingSeconds % 10;
        int firstSecondsDigit = remainingSeconds / 10;
        int finalNumber = lastSecondsDigit + firstSecondsDigit;
        if(finalNumber == firstDigitOfSerial)
        {
          Debug.LogFormat("[Captain Wheeler " + "#" + ModuleId + "] Module solved!", ModuleId);
          SolveDisplays();
          Solve();
          Audio.PlaySoundAtTransform("Solve", Buttons[0].transform);
        }
        else
        {
          Debug.LogFormat("[Captain Wheeler " + "#" + ModuleId + "] Strike! Defuser pressed at time: " + firstSecondsDigit + lastSecondsDigit + " instead of adding up both to: " + firstDigitOfSerial + ".", ModuleId);
          Strike();
          Audio.PlaySoundAtTransform("Strike", Buttons[0].transform);
        }
        break;

        case 2: //black and white
        int remainingSeconds1 = (int)Timer % 60;
        int lastSecondsDigit2 = remainingSeconds1 % 10;
        int firstSecondsDigit2 = remainingSeconds1 / 10;
        if((lastSecondsDigit2 == 1 || lastSecondsDigit2 == 0) && (firstSecondsDigit2 == 1 || firstSecondsDigit2 == 0))
        {
          Debug.LogFormat("[Captain Wheeler " + "#" + ModuleId + "] Module solved!", ModuleId);
          SolveDisplays();
          Solve();
          Audio.PlaySoundAtTransform("Solve", Buttons[0].transform);
        }
        else
        {
          Debug.LogFormat("[Captain Wheeler " + "#" + ModuleId + "] Strike! Defuser pressed at time: " + firstSecondsDigit2 + lastSecondsDigit2 + " instead of 11, 10, 01, or 00.", ModuleId);
          Strike();
          Audio.PlaySoundAtTransform("Strike", Buttons[0].transform);
        }
        break;

        case 3: //normal
        int remainingDigit = (int)Timer % 10;
        IEnumerable<int> numbersInSerial = Bomb.GetSerialNumberNumbers();
        int[] numberArray = numbersInSerial.ToArray();
        int dRoot = DigitalRoot(numberArray);
        if(dRoot == remainingDigit)
        {
          Debug.LogFormat("[Captain Wheeler " + "#" + ModuleId + "] Module solved!", ModuleId);
          SolveDisplays();
          Solve();
          Audio.PlaySoundAtTransform("Solve", Buttons[0].transform);
        }
        else
        {
          Debug.LogFormat("[Captain Wheeler " + "#" + ModuleId + "] Strike! Defuser pressed at time: " + remainingDigit + " instead of " + dRoot + ".", ModuleId);
          Strike();
          Audio.PlaySoundAtTransform("Strike", Buttons[0].transform);
        }
        break;
      };
    }

    int DigitalRoot(int[] numbers){
      string digitsString = "";
      foreach(int number in numbers)
      {
        digitsString += number.ToString();
      }
      uint parseNum = uint.Parse(digitsString);
      while (parseNum / 10 != 0)
            {
                uint sum = 0;
                int i = 10;
                int j = 1;

                while (parseNum / j >= 1)
                {
                    sum += (uint)(parseNum % i / j);

                    i *= 10;
                    j *= 10;
                }

                parseNum = sum;
            }

            return (int)parseNum;
        }

   void Start () { //Shit
     WriteStrings(); //initalizies the set of ship names and captains
     WriteShipOrders(); //initalizies the set of ship orders for the tables
     wheel.Spin(); //spins wheel forever
     RandomChoices(); //random choices for the buttons to be on, and hat
     DetermineCorrectCaptain(); // captain
     DetermineCorrectShip(); // ships
     BeforeDetermineCorrectTime();
   }

void WriteShipOrders()
{
  shipsReadingOrder = new string[]
  {
    "SS Dulcibella",
    "HMS Fizze",
    "Borealis",
    "Lady Lumbridge",
    "MS Jewel",
    "OFS Kestrel",
    "Jericho",
    "Flying Dutchman",
    "MS Pearl",
    "Skeld"
  };

  shipsBrailleOrder = new string[]
  {
    "SS Dulcibella",
    "OFS Kestrel",
    "HMS Fizze",
    "Jericho",
    "Borealis",
    "Flying Dutchman",
    "Lady Lumbridge",
    "MS Pearl",
    "MS Jewel",
    "Skeld"
  };

  shipsChineseOrder = new string[]
  {
    "MS Jewel",
    "Skeld",
    "Lady Lumbridge",
    "MS Pearl",
    "Borealis",
    "Flying Dutchman",
    "HMS Fizze",
    "Jericho",
    "SS Dulcibella",
    "OFS Kestrel"
  };
}

void ChangeFontSize() {
  //blackbeard
  if (currentSailorIndex == 6)
  {
      DisplayTexts[0].fontSize = 80;
  }
  else
  {
      DisplayTexts[0].fontSize = 100;
  }

  //ship
  if (currentShipIndex == 0 || currentShipIndex == 3 || currentShipIndex == 7)
  {
      DisplayTexts[1].fontSize = 80;
  }
  else
  {
      DisplayTexts[1].fontSize = 100;
  }
}

   void WriteStrings() { //Didn't want this in start
   captainNames = new string[]
      {
          "Cpt. Wheeler",
          "Adm. Bubbles",
          "Cpt. Squiggle",
          "Adm. Flappa",
          "Cpt. Morgan",
          "Adm. Snicker",
          "Cpt. Blackbeard",
          "Adm. Quirky",
          "Cpt. Jack",
          "Davy Jones"
      };

      shipNames = new string[]
      {
        "SS Dulcibella",
        "HMS Fizze",
        "Borealis",
        "Lady Lumbridge",
        "MS Jewel",
        "OFS Kestrel",
        "Jericho",
        "Flying Dutchman",
        "MS Pearl",
        "Skeld"
      };
   }
   void Update () { //Shit that happens at any point after initialization

   }

   void SolveDisplays()
   {
     wheel.ComeBackUp();
     hat.GoBackDown();
     ModuleSolved=true;
   }

   void Solve () {
      GetComponent<KMBombModule>().HandlePass();
   }

   void Strike () {
      GetComponent<KMBombModule>().HandleStrike();
   }

#pragma warning disable 414
   private readonly string TwitchHelpMessage = @"Use !{0} press wheel to press the wheel. Use !{0} name [left/right] to go through the sailor names. Use !{0} ship [left/right] to go through the ship names. Use !{0} sail at [00] to sail at the specified time.";
#pragma warning restore 414

//Really shit way of doing this, I think
IEnumerator ProcessTwitchCommand(string input)
{
    var cmd = input.ToLowerInvariant().Split(' ').ToArray();

    if (cmd[0] == "sail")
    {
      if (cmd.Length != 3 || cmd[1] != "at" || !IsTimeValid(cmd[2]))
      {
          Debug.Log("Cannot sail at the time: " + input);
          yield break;
      }
      string targetTime = cmd[2];
      yield return null;
      while ((((int)Bomb.GetTime()) % 60).ToString("D2") != targetTime)
                  yield return "trycancel The command to perform the action was cancelled due to a cancel request.";

              if ((((int)Bomb.GetTime()) % 60).ToString("D2") == targetTime)
                  Buttons[5].OnInteract();

              yield break;
    }

    else if (cmd[0] == "press" && cmd.Length == 2)
    {
        string button = cmd[1];

        switch (button)
        {
            case "wheel":
                Buttons[0].OnInteract();
                yield return "wheel";
                break;

            default:
                Debug.Log("Invalid command: " + input);
                break;
        }
      }

      else if (cmd[0] == "name" && cmd.Length == 2)
      {
          string button = cmd[1];
          switch (button)
          {
              case "left":
                  Buttons[1].OnInteract();
                  yield return "name";
                  break;

                  case "right":
                      Buttons[2].OnInteract();
                      yield return "name";
                      break;

              default:
                  Debug.Log("Invalid command: " + input);
                  break;
          }
        }

        else if (cmd[0] == "ship" && cmd.Length == 2)
        {
            string button = cmd[1];
            switch (button)
            {
                case "left":
                    Buttons[3].OnInteract();
                    yield return "ship";
                    break;

                    case "right":
                        Buttons[4].OnInteract();
                        yield return "ship";
                        break;

                default:
                    Debug.Log("Invalid command: " + input);
                    break;
            }
          }
  else
  {
    //nothing
    }
  }

bool IsTimeValid(string time)
{
    int seconds;
    if (time.Length != 2 || !int.TryParse(time, out seconds) || seconds < 0 || seconds > 59)
        return false;
    return true;
}

string GetCurrentTime()
{
    int currentTime = (int)Bomb.GetTime();
    return currentTime.ToString("D2");
}


   IEnumerator TwitchHandleForcedSolve () {
     //Don't feel like writing code to make it look like its actually doing it, so forcing a solve will literally force it to solve
     if(wheelDown)
     {
       SolveDisplays();
     }
     else
     {
       //nochange of displays if wheel is already up
     }
     Solve();
     Audio.PlaySoundAtTransform("Solve", Buttons[0].transform);
      yield return null;
   }
}
