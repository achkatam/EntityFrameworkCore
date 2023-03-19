﻿namespace ProductShop.DTOs.Export;

using System.Xml.Serialization;

[XmlType("User")]
public class UserDto
{
    [XmlElement("firstName")]
    public string FirstName { get; set; } = null!;

    [XmlElement("lastName")]
    public string LastName { get; set; } = null!;

    [XmlElement("age")]
    public int? Age { get; set; }


    public SoldProducts SoldProducts { get; set; }
}