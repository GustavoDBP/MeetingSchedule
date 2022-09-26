using MeetingSchedule.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace MeetingSchedule.DBContext
{
    public static class MeetingScheduleBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User() { UserName = "UsuarioA", Password = "123456", Role = "admin" }
            );

            modelBuilder.Entity<Room>().HasData(
                 new Room() { Id = 1, Name = "Sala 1", Color = "#00ff99" },
                 new Room() { Id = 2, Name = "Sala 2", Color = "#ff9933" },
                 new Room() { Id = 3, Name = "Sala 3", Color = "#1e81b0" }
             );

            modelBuilder.Entity<Meeting>().HasData(
                new Meeting() { Id = 1, Name = "Reunião 1", Description = "Descrição da reunião 1", Start = new DateTime(2022, 9, 20, 10, 0, 0), End = new DateTime(2022, 9, 20, 11, 0, 0), RoomId = 1 },
                new Meeting() { Id = 2, Name = "Reunião 2", Description = "Descrição da reunião 2", Start = new DateTime(2022, 9, 20, 10, 0, 0), End = new DateTime(2022, 9, 20, 11, 0, 0), RoomId = 2 },
                new Meeting() { Id = 3, Name = "Reunião 3", Description = "Descrição da reunião 3", Start = new DateTime(2022, 9, 20, 14, 30, 0), End = new DateTime(2022, 9, 20, 15, 0, 0), RoomId = 1 },
                new Meeting() { Id = 4, Name = "Reunião 4", Description = "Descrição da reunião 4", Start = new DateTime(2022, 9, 21, 10, 0, 0), End = new DateTime(2022, 9, 21, 11, 0, 0), RoomId = 1 }
            );
        }
    }
}
