using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace pet_hotel
{
    public enum PetBreedType {
        Poodle,     //0
        Shephard,   //1
        Terrier,    //2
        Bulldog,    //3
        Boxer,      //4
        Labrador,   //5
        Retriver    //6
    }
    public enum PetColorType {
        Black,       //0
        Tricolor,    //1
        Spotted,     //2
        Golden,      //3
        White        //4
    }
    public class Pet {
        public int id {get; set; }

        [Required]
        public string name {get; set; }

        public PetOwner petOwner { get; set; }

        public int petOwnerid { get; set; }

        public DateTime checkedInAt { get; set; }
    
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PetBreedType breed { get; set; }
        
        [JsonConverter(typeof(JsonStringEnumConverter))]    
        public PetColorType color { get; set; }

        public bool checkedIn { get; set; }
        public void petCheckIn(){
            checkedIn = true;
            
        }

        public void petCheckOut(){
            checkedIn = false;
        }
    }
}
