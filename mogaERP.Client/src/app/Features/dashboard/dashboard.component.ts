import { Component } from '@angular/core';
import { Chart } from 'angular-highcharts';
import { ChartModule } from 'angular-highcharts';
@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent {
  finStatChart!: Chart;
  projectsChart!: Chart;
  servicesChart!: Chart;
  expensesByTypeChart!: Chart;
  revenuesByDeptChart!: Chart;
  clientGrowthChart!:Chart;
  // 
  topClients = [
    { name: 'شركة ABC', projects: 5, revenue: 120000, image: 'assets/client1.png' },
    { name: 'مؤسسة XYZ', projects: 3, revenue: 80000, image: 'assets/client2.png' },
    { name: 'عميل خاص', projects: 2, revenue: 45000, image: 'assets/client3.png' }
  ];
  
  topEmployees = [
    { name: 'أحمد علي', role: 'مطور ويب', projects: 10, image: 'assets/emp1.png' },
    { name: 'سارة محمد', role: 'مصممة UI/UX', projects: 8, image: 'assets/emp2.png' },
    { name: 'محمود حسن', role: 'مسوق رقمي', projects: 6, image: 'assets/emp3.png' }
  ];
  
  lastProjects = [
    { name: 'موقع شركة ABC', status: 'مكتمل' },
    { name: 'تطبيق XYZ', status: 'جاري' },
    { name: 'حملة تسويق 2025', status: 'مكتمل' }
  ];
  ngOnInit(): void {
    this.renderFinancialChart();
    this.renderProjectsChart();
    this.renderServicesChart();
    this.renderExpensesByTypeChart();
    this.renderRevenuesByDeptChart();
    this.renderClientGrowthChart();
  }
  
  renderFinancialChart() {
    this.finStatChart = new Chart({
      chart: {
        type: 'pie',
        renderTo: 'fin-stat',
        height: '100%',
        width: undefined,
        style: { fontFamily: 'Tajawal' }
      },
      title: {
        useHTML: true,
        text: 'الوضع المالي<br><span style="font-size:24px; font-weight:bold; color:#3D5DA7">30K</span>',
        align: 'center',
        verticalAlign: 'middle',
        y: 10,
        style: { fontSize: '16px', fontWeight: 'bold', color: '#0d6efd' }
      },      
      tooltip: { enabled: false },
      accessibility: { enabled: false },
      credits: { enabled: false },
      plotOptions: {
        pie: {
          innerSize: '70%',
          dataLabels: { enabled: false }
        }
      },
      legend: {
        align: 'center',
        verticalAlign: 'bottom',
        layout: 'horizontal',
        itemStyle: {
          fontSize: '14px',
          fontWeight: 'bold'
        },
        // labelFormatter: function () {
        //   if (this.name === 'إيرادات') {
        //     return 'إيرادات (أخضر)';
        //   }
        //   if (this.name === 'مصروفات') {
        //     return 'مصروفات (أحمر)';
        //   }
        //   return this.name;
        // }
      },
      
      series: [
        {
          type: 'pie',
          data: [
            { name: 'Profits', y: 20000, color: '#34C759' },
            { name: 'Expenses', y: 10000, color: '#FF3B30' }
          ]
        }
      ]
    });
  }

  renderProjectsChart() {
    this.projectsChart = new Chart({
      chart: {
        type: 'pie',
        renderTo: 'projects-stat',
        height: '100%',
        width: undefined,
        style: { fontFamily: 'Tajawal' }
      },
      title: {
        useHTML: true,
        text: 'عدد المشاريع<br><span style="font-size:24px; font-weight:bold; color:#3D5DA7">300</span>',
        align: 'center',
        verticalAlign: 'middle',
        y: 10,
        style: { fontSize: '16px', fontWeight: 'bold', color: '#0d6efd' }
      },  
      tooltip: { enabled: false },
      accessibility: { enabled: false },
      credits: { enabled: false },
      plotOptions: {
        pie: {
          innerSize: '70%',
          dataLabels: { enabled: false }
        }
      },
      legend: {
        align: 'right',
        verticalAlign: 'middle',
        layout: 'vertical'
      },
      series: [
        {
          type: 'pie',
          data: [
            { name: 'Completed', y: 2500, color: '#34C759' },
            { name: 'Closed', y: 500, color: '#FF3B30' }
          ]
        }
      ]
    });
  }

  renderServicesChart() {
    const categories = [
      'ويب',
      'موبايل',
      'تسويق إلكتروني',
      'كاميرات مراقبة',
      'ماكينات تصوير',
      'بلوتر'
    ];

    const currentData = [20, 35, 40, 25, 15, 10];
    const previousData = [15, 30, 35, 20, 12, 8];

    this.servicesChart = new Chart({
      chart: {
        type: 'area',
        style: {
          fontFamily: 'Tajawal',
        },
      },
      accessibility: { enabled: false },
      title: { 
        text: 'تحليل أداء الخدمات',
        style: { fontSize: '18px', fontWeight: 'bold' , textAlign: 'start' }
      },
      xAxis: {
        categories: categories,
        title: { text: '' }
      },
      yAxis: {
        title: { text: 'القيمة' }
      },
      tooltip: {
        shared: true,
      },
      plotOptions: {
        area: {
          fillOpacity: 0.4,
        },
      },
      series: [
        {
          name: 'الشهر الحالي',
          data: currentData,
          type: 'area',
          color: '#3D5DA7',
        },
        {
          name: 'الشهر الماضي',
          data: previousData,
          type: 'area',
          color: '#FAA21A',
        },
      ],
      legend: {
        align: 'center',
        verticalAlign: 'bottom'
      },
      credits: { enabled: false },
    });
  }

  renderExpensesByTypeChart() {
    this.expensesByTypeChart = new Chart({
      chart: {
        type: 'pie',
        height: '100%',
        width: undefined,
        style: { fontFamily: 'Tajawal' }
      },
      title: { text: 'المصروفات حسب النوع' },
      accessibility: { enabled: false },
      tooltip: {
        pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
      },
      plotOptions: {
        pie: {
          allowPointSelect: true,
          cursor: 'pointer',
          dataLabels: {
            enabled: true,
            format: '<b>{point.name}</b>: {point.percentage:.1f} %'
          }
        }
      },
      series: [
        {
          name: 'المصروفات',
          type: 'pie',
          data: [
            { name: 'رواتب', y: 45 },
            { name: 'تسويق', y: 25 },
            { name: 'صيانة', y: 15 },
            { name: 'مصاريف مكتبية', y: 10 },
            { name: 'أخرى', y: 5 }
          ]
        }
      ],
      credits: { enabled: false }
    });
  }

  renderRevenuesByDeptChart() {
    this.revenuesByDeptChart = new Chart({
      chart: {
        type: 'column',
        style: { fontFamily: 'Tajawal' }
      },
      title: { text: 'الأرباح حسب الأقسام' },
      accessibility: { enabled: false },
      xAxis: {
        categories: ['المبيعات', 'برامج الموبايل', 'الويب', 'التسويق الرقمي'],
        crosshair: true
      },
      yAxis: {
        min: 0,
        title: { text: 'القيمة' }
      },
      tooltip: {
        headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
        pointFormat:
          '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
          '<td style="padding:0"><b>{point.y:.0f} جنيه</b></td></tr>',
        footerFormat: '</table>',
        shared: true,
        useHTML: true
      },
      plotOptions: {
        column: {
          borderRadius: 5,
          pointPadding: 0.2,
          borderWidth: 0
        }
      },
      series: [
        {
          name: 'الأرباح',
          type: 'column',
          data: [50000, 30000, 40000, 25000],
          color: '#3D5DA7'
        }
      ],
      credits: { enabled: false }
    });
  }
  
  renderClientGrowthChart() {
    this.clientGrowthChart = new Chart({
      chart: {
        type: 'area',
        style: { fontFamily: 'Tajawal' }
      },
      title: { text: 'نمو العملاء خلال العام' },
      accessibility: { enabled: false },
      xAxis: {
        categories: [
          'يناير', 'فبراير', 'مارس', 'أبريل', 'مايو', 'يونيو',
          'يوليو', 'أغسطس', 'سبتمبر', 'أكتوبر', 'نوفمبر', 'ديسمبر'
        ],
        tickmarkPlacement: 'on'
      },
      yAxis: {
        title: { text: 'عدد العملاء' }
      },
      tooltip: {
        shared: true,
        valueSuffix: ' عميل'
      },
      plotOptions: {
        area: {
          fillOpacity: 0.5,
          marker: {
            enabled: true,
            radius: 4
          }
        }
      },
      series: [
        {
          name: 'عدد العملاء',
          type: 'area',
          data: [120, 150, 180, 210, 300, 450, 500, 650, 700, 850, 950, 1100],
          color: '#FAA21A'
        }
      ],
      credits: { enabled: false }
    });
  }
  
}
