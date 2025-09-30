import { Component, EventEmitter, Output } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs';
import { MenuService } from '../../core/services/menu.service';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.css',
})
export class LayoutComponent {
  routeNow: string = '';
  currentDate: string = '';
  menusList!: any;
  isCollapsed: boolean = false;
  activeMenu: string | null = null;
  activeChildMenu: string | null = null;
  activeSubMenu: string = '';
  activeRoute: string = '';
  permissions: { [key: string]: boolean } = {};
  RoleName: string | null = null;

  @Output() sidebarToggled = new EventEmitter<boolean>();

  constructor(
    private router: Router,
    private menuService: MenuService
  ) {
    this.menusList = this.menuService.getFilteredMenus();
  }

  ngOnInit() {
    this.RoleName = sessionStorage.getItem('role');
    this.setCurrentDate();
    this.routeNow = this.getArabicRouteName(this.router.url);
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationEnd) {
        this.routeNow = this.getArabicRouteName(event.url);
      }
    });
    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe(() => {
        this.setActiveMenuBasedOnRoute();
      });

    this.setActiveMenuBasedOnRoute();
  }

  setActiveMenuBasedOnRoute() {
    const currentRoute = this.router.url;
    this.activeMenu = null;
    this.activeChildMenu = null;
    for (const main of this.menusList) {
      if (main.route && currentRoute.includes(main.route)) {
        this.activeMenu = main.displayName;
      }
      if (main.subMenus) {
        for (const child of main.subMenus) {
          if (child.route && currentRoute.includes(child.route)) {
            this.activeMenu = main.displayName;
            this.activeChildMenu = child.displayName;
            return;
          }
          if (child.subMenus) {
            for (const subChild of child.subMenus) {
              if (subChild.route && currentRoute.includes(subChild.route)) {
                this.activeMenu = main.displayName;
                this.activeChildMenu = child.displayName;
                return;
              }
            }
          }
        }
      }
    }
  }

  toggleMainMenu(menu: string) {
    this.activeMenu = this.activeMenu === menu ? null : menu;
    this.activeChildMenu = null;
  }

  toggleChildMenu(childMenu: string) {
    this.activeChildMenu = this.activeChildMenu === childMenu ? null : childMenu;
  }

  toggleSidebar() {
    this.isCollapsed = !this.isCollapsed;
    this.sidebarToggled.emit(this.isCollapsed);
  }

  
  setCurrentDate(): void {
    const today = new Date();
    const day = today.getDate();
    const month = today.toLocaleString('ar-EG', { month: 'short' });
    const weekday = today.toLocaleString('ar-EG', { weekday: 'short' });
    this.currentDate = `اليوم، ${weekday} ${day} ${month}`;
  }

  getArabicRouteName(url: string): string {
    let path = url;
    try {
      if (url.includes('://')) {
        const urlObj = new URL(url);
        path = urlObj.pathname;
      }
      path = path.split('?')[0].split('#')[0];
      path = path.startsWith('/') ? path : `/${path}`;
      path = path.endsWith('/') ? path.slice(0, -1) : path;
    } catch (e) {
      console.error('Invalid URL format:', url);
    }
    const doctorsDynamicRoutePattern = /^\/settings\/doctors\/\d+$/;
    if (doctorsDynamicRoutePattern.test(path)) {
      return 'إعدادات الشركة';
    }
    switch (path) {
      case '/':
      case '/dashboard':
        return 'لوحة البيانات';
      case '/staff/list':
        return 'إدارة الموظفين';
      case '/staff/add':
        return 'إضافة موظف';
      case 'staff/department-admin':
        return 'إدارة الأقسام';
      case '/staff/job-levels':
        return 'إدارة المستويات الوظيفية';
      case '/staff/job-classification':
        return 'إدارة التصنيف الوظيفي';
      case '/staff/job-management':
        return 'إدارة الوظائف';
      case '/reports':
        return 'التقارير';
      case '/settings/doctors':
        return 'إعدادات الأطباء';
      case '/settings/doctors-list':
        return 'إدارة الأطباء';
      case '/settings/medical-services-list':
        return 'إدارة الخدمات الطبية';
      case '/settings/medical-departments-list':
        return 'إدارة الأقسام';
      case '/settings/account-tree':
        return 'شجرة الحسابات'
      case 'settings/cost-center-tree':
        return 'مراكز التكلفة'
      case 'fin-tree/year-fin-settings':
        return 'إعدادات السنة المالية'
      case '/staff/progression':
        return 'إدارة التدرج الوظيفي';
      case '/staff/classification':
        return 'إدارة التصنيف الوظيفي';
      case '/insurance/insurance-list':
        return 'إدارة التأمينات';
      case '/insurance/add-insurance':
        return 'إضافة وكيل تأمين';
      case '/system-settings/stores-settings/main-groups':
        return 'المجموعات الرئيسية';
      case '/system-settings/stores-settings/items-group':
        return 'مجموعة الأصناف';
      case '/system-settings/stores-settings/items':
        return 'الأصناف';
      case '/system-settings/stores-settings/items-units':
        return 'وحدات الأصناف';
      case '/system-settings/providers':
        return 'الموردين';
      case '/system-settings/customers':
        return 'العملاء';
      case '/system-settings/boxes':
        return 'الخزائن';
      case '/system-settings/banks':
        return 'البنوك';
      case '/system-settings/accounts':
        return 'الحسابات العامة';
      case '/fin-tree/add-items':
        return 'أذون الإستلام';
      case '/fin-tree/issue-items':
        return 'أذون الصرف';
      case '/fin-tree/issue-request':
        return 'طلبات الصرف';
      case '/fin-tree/fetch-inventory':
        return 'جرد المخازن';
      case '/fin-tree/treasury':
        return 'حركة الخزينة';
      case '/fin-tree/treasury/supply-receipt':
        return 'إيصال توريد';
      case '/fin-tree/treasury/exchange-permission':
        return 'إذن صرف نقدي';
      case '/fin-tree/bank':
        return 'حركة البنوك';
      case '/fin-tree/bank/add-notice':
        return 'إشعار إضافة';
      case '/fin-tree/bank/discount-notice':
        return 'إشعار خصم';
      case '/fin-tree/restrictions':
        return 'قيود اليومية';
      case '/fin-tree/purchase-order':
        return 'أوامر توريد';
      case '/purchases/purchase-request':
        return 'طلبات الشراء';
      case '/purchases/price-quotations':
        return 'عروض الأسعار';
      case '/purchases/purchase-order':
        return 'أوامر توريد';
      default:
        console.warn('No matching route found for:', path);
        return 'Infinity ERP';
    }
  }

  logout() {
    // Swal.fire({
    //   title: 'هل أنت متأكد؟',
    //   icon: 'question',
    //   showCancelButton: true,
    //   confirmButtonColor: '#3D5DA7',
    //   cancelButtonColor: '#ED3B93',
    //   confirmButtonText: 'نعم',
    //   cancelButtonText: 'لا'
    // }).then((result) => {
    //   if (result.isConfirmed) {
    //     this.authService.logout();
    //     this.router.navigate(['/login']);
    //   }
    // });
  }
}
