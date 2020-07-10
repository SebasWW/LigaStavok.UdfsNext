using Udfs.Transmitter.Messages.Identifiers;

namespace LigaStavok.UdfsNext.Provider.SportLevel.Adapter.Converters
{
    public static class SportConverter
    {
        public static Sport Convert(long sportId)
        {
            switch (sportId)
            {
                case 1:   return Sport.Football;
                case 2:   return Sport.IceHockey; // Basketball;
                case 3:   return Sport.Basketball; //Baseball;
                case 4:   return Sport.Futsal;
                case 5:   return Sport.Boxing; //Sport.Tennis;
                case 6:   return Sport.TableTennis; 
                case 7:   return Sport.Handball; // Floorball;
                // case 8:   return Sport.Billiard;
                case 9:   return Sport.ESportCounterStrike; // Sport.Golf;
                case 10:  return Sport.ESportWorldOfTanks; // Sport.Boxing;
                case 11:  return Sport.Volleyball; // MotorSport;
                //case 12:  return Sport.Rugby;
                case 13:  return Sport.ESportDota; //AustralianFootball;
                case 14:  return Sport.ESportHearthstone; // WinterSports;
                case 15:  return Sport.ESportLeagueOfLegends; // Bandy;
                //case 16:  return Sport.AmericanFootball;
                //case 17:  return Sport.Cycling;
                case 18:  return Sport.BeachSoccer; // Specials;
                //case 19:  return Sport.Snooker;
                case 20:  return Sport.Badminton;
                //case 21:  return Sport.Cricket;
                case 22:  return Sport.Floorball; // Darts;
                case 23:  return Sport.BeachVolleyball;
                case 24:  return Sport.Waterpolo; 
                case 25:  return Sport.Bandy; //  Pool;
                case 26:  return Sport.FieldHockey;
                //case 27:  return Sport.GaelicSports;
                //case 28:  return Sport.Curling;
                //case 29:  return Sport.Futsal;
                //case 30:  return Sport.Olympics;
                //case 31:  return Sport.Badminton;
                //case 32:  return Sport.Bowls;
                //case 33:  return Sport.Chess;
                //case 34:  return Sport.BeachVolleyball;
                case 35:  return Sport.ESport; // Netball;
                case 36:  return Sport.Rugby; // Athletics;
                case 37:  return Sport.AmericanFootball; // Squash;
                //case 38:  return Sport.RinkHockey;
                //case 39:  return Sport.Lacrosse;
                //case 40:  return Sport.Formula1;
                //case 41:  return Sport.Bikes;
                //case 42:  return Sport.DTM;
                //case 43:  return Sport.AlpineSkiing;
                case 44:  return Sport.Tennis; // Biathlon;
                //case 45:  return Sport.Bobsleigh;
                //case 46:  return Sport.CrossCountry;
                //case 47:  return Sport.NordicCombined;
                //case 48:  return Sport.SkiJumping;
                //case 49:  return Sport.Snowboard;
                //case 50:  return Sport.SpeedSkating;
                //case 51:  return Sport.Luge;
                //case 52:  return Sport.Swimming;
                //case 53:  return Sport.FinnishBaseball;
                //case 54:  return Sport.Softball;
                //case 55:  return Sport.HorseRacing;
                //case 56:  return Sport.Schwingen;
                //case 57:  return Sport.InlineHockey;
                //case 58:  return Sport.Greyhound;
                //case 59:  return Sport.RugbyLeague;
                //case 60:  return Sport.BeachSoccer;
                //case 61:  return Sport.Pesapallo;
                //case 62:  return Sport.StreetHockey;
                //case 63:  return Sport.WorldChampionship;
                //case 64:  return Sport.Rowing;
                //case 65:  return Sport.Freestyle;
                //case 68:  return Sport.Moto2;
                //case 69:  return Sport.Moto3;
                //case 70:  return Sport.Nascar;
                //case 66:  return Sport.SnowboardcrossParallel;
                //case 67:  return Sport.MotoGP;
                //case 71:  return Sport.PadelTennis;
                //case 72:  return Sport.Canoeing;
                //case 73:  return Sport.Horseball;
                //case 74:  return Sport.Aquatics;
                //case 75:  return Sport.Archery;
                //case 76:  return Sport.Equestrian;
                //case 77:  return Sport.Fencing;
                //case 78:  return Sport.Gymnastics;
                //case 79:  return Sport.Judo;
                //case 80:  return Sport.ModernPentathlon;
                //case 81:  return Sport.Sailing;
                //case 82:  return Sport.Shooting;
                //case 83:  return Sport.Taekwondo;
                //case 84:  return Sport.Triathlon;
                //case 85:  return Sport.Weightlifting;
                //case 86:  return Sport.Wrestling;
                //case 87:  return Sport.OlympicsYouth;
                //case 88:  return Sport.MountainBike;
                //case 89:  return Sport.Riding;
                //case 90:  return Sport.Surfing;
                //case 91:  return Sport.BMXRacing;
                //case 92:  return Sport.CanoeSlalom;
                //case 93:  return Sport.RhythmicGymnastics;
                //case 94:  return Sport.Trampolining;
                //case 95:  return Sport.SynchronizedSwimming;
                //case 96:  return Sport.Diving;
                //case 97:  return Sport.TrackCycling;
                //case 98:  return Sport.BeachTennis;
                //case 99:  return Sport.Sumo;
                //case 100: return Sport.Superbike;
                //case 101: return Sport.Rally;
                //case 102: return Sport.FigureSkating;
                //case 103: return Sport.FreestyleSkiing;
                //case 104: return Sport.Skeleton;
                //case 105: return Sport.ShortTrack;
                //case 106: return Sport.ESportMythicalFootball;
                //case 107: return Sport.ESport;
                //case 108: return Sport.WorldLottery;
                //case 109: return Sport.ESportCounterStrike;
                //case 110: return Sport.ESportLeagueOfLegends;
                //case 111: return Sport.ESportDota;
                //case 112: return Sport.ESportStarCraft;
                //case 113: return Sport.ESportHearthstone;
                //case 114: return Sport.ESportHeroesOfTheStorm;
                //case 115: return Sport.ESportWorldOfTanks;
                //case 116: return Sport.Polo;
                //case 117: return Sport.MMA;
                //case 118: return Sport.ESportCallOfDuty;
                //case 119: return Sport.ESportSmite;
                //case 120: return Sport.ESportVainglory;
                //case 121: return Sport.ESportOverwatch;
                //case 122: return Sport.ESportWarCraft3;
                //case 123: return Sport.ESportCrossfire;
                //case 124: return Sport.ESportHalo;
                //case 125: return Sport.ESportRainbowSix;
                //case 126: return Sport.SepakTakraw;
                default:
                    return null;
            }
        }
    }
}