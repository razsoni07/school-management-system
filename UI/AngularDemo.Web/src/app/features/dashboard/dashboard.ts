import { Component, OnInit } from '@angular/core';
import { Auth } from '../../services/auth';
import { DashboardService } from '../../services/dashboard-service';
import { CommonModule } from '@angular/common';

import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { NgChartsModule } from 'ng2-charts';
import { ChartConfiguration, ChartOptions } from 'chart.js';

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule, MatCardModule, MatIconModule, NgChartsModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard implements OnInit {
  stats: any = {};
  role = '';
  today = new Date();

  // Pie Chart Data
  public pieChartData: ChartConfiguration<'pie'>['data'] = {
    labels: ['Students', 'Teachers', 'Classes', 'Subjects'],
    datasets: [{
      data: [0, 0, 0, 0],
      backgroundColor: [
        '#3f51b5',
        '#4caf50',
        '#ff9800',
        '#e91e63'
      ]
    }]
  };

  public pieChartOptions: ChartOptions<'pie'> = {
    responsive: true,
    plugins: {
      legend: {
        position: 'bottom'
      }
    }
  };

  constructor(private dashboardService: DashboardService, public auth: Auth) {}

  ngOnInit(): void {
    debugger
    const user = this.auth.getUserInfo();
    this.role = user?.role;

    if (this.role === 'Admin') {
      this.dashboardService.getAdminStats().subscribe(res => {
        this.stats = res
        this.loadChartData();
      });
    }

    if (this.role === 'Principal') {
      this.dashboardService.getPrincipalStats().subscribe(res => {
        this.stats = res
        this.loadChartData();
      });
    }
  }

  loadChartData() {
    // Adjust property names based on your API response
    this.pieChartData.datasets[0].data = [
      this.stats.totalStudents || 0,
      this.stats.totalTeachers || 0,
      this.stats.totalClasses || 0,
      this.stats.totalSubjects || 0
    ];
  }
}
