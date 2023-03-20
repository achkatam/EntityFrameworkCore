﻿namespace CarDealer.DTOs.Export.Car;
using System.Xml.Serialization;

[XmlType("car")]
public class ExportCarBmwDto
{
    [XmlAttribute("id")]
    public int Id { get; set; }

    [XmlAttribute("model")]
    public string Model { get; set; } = null!;

    [XmlAttribute("traveled-distance")]
    public long TraveledDistance { get; set; }
}