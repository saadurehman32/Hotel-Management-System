using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace HotelManagement
{
    public class RoomBookingService
    {
        private readonly IJSRuntime _jsRuntime;

        private List<ModelRoom> rooms = new List<ModelRoom>();

        public List<ModelRoom> Rooms { get; private set; } = new List<ModelRoom>();

        public event Action OnChange;

        public RoomBookingService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
            LoadRoomsFromLocalStorageAsync().Wait();
        }

        public async Task<string> AddRoomAsync(ModelRoom room)
        {
            if (rooms.Any(r => r.RoomNumber == room.RoomNumber))
            {
                return "Room number already exists!";
            }

            rooms.Add(room);
            Rooms.Add(room);

            await SaveRoomsToLocalStorageAsync();
            NotifyStateChanged();

            return "Room added successfully!";
        }

        public async Task<string> DeleteRoomAsync(int roomNumber)
        {
            var room = rooms.FirstOrDefault(r => r.RoomNumber == roomNumber);
            if (room == null)
            {
                return "Room not found!";
            }

            rooms.Remove(room);
            Rooms.Remove(room);

            await SaveRoomsToLocalStorageAsync();
            NotifyStateChanged();

            return "Room deleted successfully!";
        }

        private async Task SaveRoomsToLocalStorageAsync()
        {
            var json = JsonSerializer.Serialize(rooms);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "rooms", json);
        }

        private async Task LoadRoomsFromLocalStorageAsync()
        {
            var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "rooms");
            if (!string.IsNullOrEmpty(json))
            {
                rooms = JsonSerializer.Deserialize<List<ModelRoom>>(json) ?? new List<ModelRoom>();
                Rooms = new List<ModelRoom>(rooms);
                NotifyStateChanged();
            }
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
