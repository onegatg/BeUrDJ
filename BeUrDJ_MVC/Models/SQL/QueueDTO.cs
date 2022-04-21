using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BeUrDJ_MVC.Models
{
    [Table("Songs")]
    public class QueueDTO
    {
        /*
        public int Id { get; set; }
        public TrackDTO[] Tracks;
        public AudioFeaturesDTO[] AudioFeatures;
        */
        [Key]
        public int SongID { get; set; }
        public string SongUri { get; set; }
        public string SongName { get; set; }
        public string ArtistName { get; set; }
        public string SongImage { get; set; }
        public int Likes { get; set; }
        public int SessionID { get; set; }
        public int PlayStateID { get; set; }
    }
}