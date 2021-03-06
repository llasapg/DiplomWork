﻿using System;
using System.ComponentModel.DataAnnotations;

namespace DiplomaSolution.Models.FileModels
{
    public class ImageFileModel
    {
        /// <summary>
        /// File id
        /// </summary>
        [Key]
        public Guid Id { get; set; }
        /// <summary>
        /// Customer accountId to map this with the file
        /// </summary>
        public string CustomerId { get; set; }
        /// <summary>
        /// Custom file name for the data provided by the customer
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// Prop to get the last added file by the customer
        /// </summary>
        public DateTime UploadTime { get; set; }
    }
}
