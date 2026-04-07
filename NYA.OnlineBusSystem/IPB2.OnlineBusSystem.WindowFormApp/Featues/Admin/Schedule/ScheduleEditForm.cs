using IPB2.OnlineBusSystem.Domain.Features.Bus;
using IPB2.OnlineBusSystem.Domain.Features.Route;
using IPB2.OnlineBusSystem.Domain.Features.Schedule;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IPB2.OnlineBusSystem.WindowFormApp.Featues.Admin.Schedule
{
    public partial class ScheduleEditForm : Form
    {
        private readonly IScheduleService _scheduleService;
        private readonly IBusService _busService;
        private readonly IRouteService _routeService;
        private readonly string _scheduleId;
        private readonly ScheduleResponse _schedule;

        public ScheduleEditForm(IScheduleService scheduleService, IBusService busService, IRouteService routeService, ScheduleResponse schedule)
        {
            _scheduleService = scheduleService;
            _busService = busService;
            _routeService = routeService;
            InitializeComponent();
            _schedule = schedule;
            _scheduleId = schedule.Id;
            LoadDataAndPopulate();
        }

        private async void LoadDataAndPopulate()
        {
            await LoadData();
            PopulateForm();
        }

        private async Task LoadData()
        {
            var buses = await _busService.GetBusComboSet();
            cboBusName.DataSource = buses;
            cboBusName.DisplayMember = "BusName";
            cboBusName.ValueMember = "Id";

            var routes = await _routeService.GetRouteComboSet();
            cboRouteName.DataSource = routes;
            cboRouteName.DisplayMember = "RouteName";
            cboRouteName.ValueMember = "Id";
        }

        private void PopulateForm()
        {
            cboBusName.SelectedValue = _schedule.BusId;
            cboRouteName.SelectedValue = _schedule.RouteId;
            dtpDate.Value = _schedule.Date.ToDateTime(TimeOnly.MinValue);
            txtFare.Text = _schedule.Fare.ToString();
            txtArrivalTime.Text = _schedule.ArrivalTime;
            txtDepartureTime.Text = _schedule.DepartureTime;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (cboBusName.SelectedValue == null || cboRouteName.SelectedValue == null ||
                string.IsNullOrWhiteSpace(txtFare.Text) || string.IsNullOrWhiteSpace(txtArrivalTime.Text) ||
                string.IsNullOrWhiteSpace(txtDepartureTime.Text))
            {
                MessageBox.Show("Please fill all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtFare.Text, out int fare))
            {
                MessageBox.Show("Fare must be a valid number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var request = new UpdateScheduleRequest
            {
                BusId = cboBusName.SelectedValue.ToString()!,
                RouteId = cboRouteName.SelectedValue.ToString()!,
                Date = DateOnly.FromDateTime(dtpDate.Value),
                Fare = fare,
                ArrivalTime = txtArrivalTime.Text,
                DepartureTime = txtDepartureTime.Text
            };

            var response = await _scheduleService.UpdateAsync(request, _scheduleId);

            if (response.Status == IPB2.OnlineBusSystem.Domain.Common.ResponseType.Success)
            {
                MessageBox.Show(response.Message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(response.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
