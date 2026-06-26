using FleetManagement.Modules.Notifications.Application;
using FleetManagement.Modules.Notifications.Domain;
using FleetManagement.WPF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FleetManagement.WPF.ViewModels
{
    public class NotificationsViewModel : ViewModelBase
    {
        private readonly INotificationRepository _notificationRepository;
        public ObservableCollection<Notification> NotificationsList { get; } = new();

        private Notification? _selectedNotification;
        public Notification? SelectedNotification { get => _selectedNotification; set => SetProperty(ref _selectedNotification, value); }

        public ICommand RefreshCommand { get; }
        public ICommand MarkAsReadCommand { get; }

        public NotificationsViewModel(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;

            RefreshCommand = new RelayCommand(async () => await LoadNotificationsAsync());
            MarkAsReadCommand = new RelayCommand(async () => await MarkAsReadAsync());

            _ = LoadNotificationsAsync();
        }

        public async Task LoadNotificationsAsync()
        {
            try
            {
                var notifications = await _notificationRepository.GetAllAsync();

                var sortedList = notifications
                    .OrderBy(x => x.IsRead)
                    .ThenByDescending(x => x.CreatedAt)
                    .ToList();

                if (Application.Current?.Dispatcher != null)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        NotificationsList.Clear();
                        foreach (var n in sortedList)
                        {
                            NotificationsList.Add(n);
                        }
                    });
                }
                else
                {
                    NotificationsList.Clear();
                    foreach (var n in sortedList)
                    {
                        NotificationsList.Add(n);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri učitavanju obaveštenja: {ex.Message}");
            }
        }

        private async Task MarkAsReadAsync()
        {
            if (SelectedNotification == null)
            {
                MessageBox.Show("Molimo izaberite obaveštenje iz tabele.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (SelectedNotification.IsRead)
            {
                return;
            }

            try
            {
                SelectedNotification.MarkAsRead();
                await _notificationRepository.UpdateAsync(SelectedNotification);
                await LoadNotificationsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška: {ex.Message}");
            }
        }
    }
}
