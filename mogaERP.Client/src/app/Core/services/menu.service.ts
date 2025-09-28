import { Injectable } from '@angular/core';
import { MenuSidebarItem } from '../models/MenuSidebarItem';

@Injectable({
  providedIn: 'root'
})
export class MenuService {
  menus: MenuSidebarItem[] = [
    {
      displayName: 'إدارة الشركة',
      icon: 'fa-solid fa-building',
      route: '/dashboard',
      pageName: 'Company',
      isGroup: false
    },
    {
      displayName: 'المشتريات',
      icon: 'fa-solid fa-boxes-stacked',
      isGroup: true,
      subMenus: [
        {
          displayName: 'طلب شراء',
          icon: 'fa-solid fa-circle',
          route: 'purchases/purchase-request',
          pageName: 'PurchaseRequest'
        },
        {
          displayName: 'عروض الأسعار',
          icon: 'fa-solid fa-circle',
          route: 'purchases/price-quotations',
          pageName: 'PriceQuotations'
        },
        {
          displayName: 'أمر توريد ',
          icon: 'fa-solid fa-circle',
          route: 'purchases/purchase-order',
          pageName: 'PurchaseOrder'
        }
      ]
    },
    {
      displayName: 'المخازن',
      icon: 'fa-solid fa-boxes-stacked',
      isGroup: true,
      subMenus: [
        {
          displayName: 'إذن إستلام ',
          icon: 'fa-solid fa-circle',
          route: 'inventory/recipt-permissions',
          pageName: 'ReciptPermissions'
        },
        {
          displayName: 'طلب صرف ',
          icon: 'fa-solid fa-circle',
          route: 'inventory/exchange-request',
          pageName: 'ExchangeRequest'
        },
        {
          displayName: 'إذن صرف',
          icon: 'fa-solid fa-circle',
          route: 'inventory/exchange-permissions',
          pageName: 'ExchangePermissions'
        }
      ]
    },
    {
      displayName: 'المبيعات',
      icon: 'fa-solid fa-chart-line',
      isGroup: true,
      subMenus: [
        {
          displayName: 'المرتجع',
          icon: 'fa-solid fa-circle',
          route: 'sales/returns',
          pageName: 'SalesReturns'
        },
        {
          displayName: 'عروض الأسعار',
          icon: 'fa-solid fa-circle',
          route: 'sales/quotations',
          pageName: 'SalesQuotations'
        },
        {
          displayName: 'الفواتير',
          icon: 'fa-solid fa-circle',
          route: 'sales/invoices',
          pageName: 'SalesInvoices'
        }
      ]
    },
    {
      displayName: 'الإدارة المالية',
      icon: 'fa-solid fa-money-bill-trend-up',
      isGroup: true,
      subMenus: [
        {
          displayName: 'حركة الخزينة',
          icon: 'fa-solid fa-circle',
          route: 'treasury',
          pageName: 'Treasury'
        },
        {
          displayName: 'حركة البنك',
          icon: 'fa-solid fa-circle',
          subMenus: [
            {
              displayName: 'اشعار اضافة',
              icon: 'fa-solid fa-circle',
              route: 'fin-tree/bank-actions/addition-notifications',
              pageName: 'AddNotice'
            },
            {
              displayName: 'اشعار خصم',
              icon: 'fa-solid fa-circle',
              route: 'fin-tree/bank-actions/debit-notifications',
              pageName: 'DiscountNotice'
            },
          ]
        },
        {
          displayName: 'القيود اليومية',
          icon: 'fa-solid fa-circle',
          route: 'restrictions',
          pageName: 'Restrictions'
        },
        {
          displayName: 'رواتب الموظفين',
          icon: 'fa-solid fa-circle',
          route: 'salaries',
          pageName: 'Salaries'
        },
        {
          displayName: 'سلف الموظفين',
          icon: 'fa-solid fa-circle',
          route: 'advances',
          pageName: 'Advances'
        }
      ]
    },
    {
      displayName: 'الموارد البشرية',
      icon: 'fa-solid fa-users-gear',
      isGroup: true,
      subMenus: [
        {
          displayName: 'بيانات الموظفين',
          icon: 'fa-solid fa-circle',
          subMenus: [
            {
              displayName: 'الموظفين',
              icon: 'fa-solid fa-circle',
              route: 'staff/list',
              pageName: 'StaffList'
            },
            {
              displayName: 'الأقسام',
              icon: 'fa-solid fa-circle',
              route: 'staff/department-admin',
              pageName: 'DepartmentAdmin'
            },
            {
              displayName: 'المستويات الوظيفية',
              icon: 'fa-solid fa-circle',
              route: 'staff/job-levels',
              pageName: 'JobLevels'
            },
            {
              displayName: 'تصنيف الوظائف',
              icon: 'fa-solid fa-circle',
              route: 'staff/classification',
              pageName: 'Classification'
            },
            {
              displayName: 'الوظائف',
              icon: 'fa-solid fa-circle',
              route: 'staff/job-management',
              pageName: 'JobManagement'
            }
          ]
        },
        {
          displayName: 'الأجازات',
          icon: 'fa-solid fa-circle',
          route: '/staff/vacation',
          pageName: 'Vacation'
        },
        {
          displayName: 'الجزاءات',
          icon: 'fa-solid fa-circle',
          route: '/staff/penalty',
          pageName: 'Penalty'
        },
        {
          displayName: 'الحضور والانصراف',
          icon: 'fa-solid fa-circle',
          route: '/staff/attendance',
          pageName: 'Attendance'
        }
      ]
    },
    {
      displayName: 'المشاريع',
      icon: 'fa-solid fa-briefcase',
      isGroup: true,
      subMenus: [
        {
          displayName: 'السوفت وير',
          icon: 'fa-solid fa-circle',
          route: '/projects/software',
          pageName: 'ProjectsSoftware'
        },
        {
          displayName: 'التسويق الرقمي',
          icon: 'fa-solid fa-circle',
          route: '/projects/digital-marketing',
          pageName: 'ProjectsDigitalMarketing'
        },
        {
          displayName: 'الهاردوير',
          icon: 'fa-solid fa-circle',
          route: '/projects/hardware',
          pageName: 'ProjectsHardware'
        }
      ]
    },
    {
      displayName: 'إدارة علاقات العملاء',
      icon: 'fa-solid fa-users',
      route: '/crm',
      pageName: 'CRM',
      isGroup: false
    },
    {
      displayName: 'إدارة المهام',
      icon: 'fa-solid fa-list-check',
      route: '/task-mangment/tasks',
      pageName: 'Tasks',
      isGroup: false
    },
    {
      displayName: 'التقارير',
      icon: 'fa-solid fa-file-invoice',
      isGroup: false,
    },
    {
      displayName: 'إعدادات النظام',
      icon: 'fa-solid fa-gear',
      isGroup: true,
      subMenus: [
        {
          displayName: 'العملاء',
          icon: 'fa-solid fa-circle',
          route: '/system-settings/customers',
          pageName: 'CustomerList'
        },
        {
          displayName: 'الموردين',
          icon: 'fa-solid fa-circle',
          route: '/system-settings/providers',
          pageName: 'ProviderList'
        },
        {
          displayName: 'المخازن',
          icon: 'fa-solid fa-circle',
          subMenus: [
            {
              displayName: 'المجموعات الرئيسية',
              icon: 'fa-solid fa-circle',
              route: '/system-settings/stores-settings/main-groups',
              pageName: 'MainGroups'
            },
            {
              displayName: 'مجموعات الاصناف',
              icon: 'fa-solid fa-circle',
              route: '/system-settings/stores-settings/items-groups',
              pageName: 'ItemsGroup'
            },
            {
              displayName: 'الوحدات',
              icon: 'fa-solid fa-circle',
              route: '/system-settings/stores-settings/item-units',
              pageName: 'Units'
            },
            {
              displayName: 'الاصناف',
              icon: 'fa-solid fa-circle',
              route: '/system-settings/stores-settings/items',
              pageName: 'Items'
            },
            {
              displayName: 'أنواع المخازن',
              icon: 'fa-solid fa-circle',
              route: '/system-settings/stores-settings/stores-types',
              pageName: 'StoreTypes'
            },
            {
              displayName: 'المخازن',
              icon: 'fa-solid fa-circle',
              route: '/system-settings/stores-settings/stores',
              pageName: 'Stores'
            },
          ]
        },
        {
          displayName: 'المالية',
          icon: 'fa-solid fa-circle',
          subMenus: [
            {
              displayName: 'شجرة الحسابات',
              icon: 'fa-solid fa-circle',
              route: '/settings/account-tree',
              pageName: 'AccountTree'
            },
            {
              displayName: 'التوجيهات المحاسبية',
              icon: 'fa-solid fa-circle',
              route: '/fin-tree/account-guidance',
              pageName: 'AccountGuidance'
            },
            {
              displayName: 'الخزائن',
              icon: 'fa-solid fa-circle',
              route: '/fin-tree/boxes',
              pageName: 'Boxes'
            },
            {
              displayName: 'البنوك',
              icon: 'fa-solid fa-circle',
              route: '/fin-tree/banks',
              pageName: 'Banks'
            },
            {
              displayName: 'السنة المالية',
              icon: 'fa-solid fa-circle',
              route: '/fin-tree/year-fin-settings',
              pageName: 'YeaFinSettings'
            },
            {
              displayName: 'مراكز التكلفة',
              icon: 'fa-solid fa-circle',
              route: '/settings/cost-center-tree',
              pageName: 'CostCenterTree'
            }
          ]
        }
      ]
    },
    {
      displayName: 'الإعدادات',
      icon: 'fa-solid fa-gear',
      isGroup: true,
      subMenus: [
        {
          displayName: 'إدارة التطبيقات',
          icon: 'fa-solid fa-circle',
          route: '/settings/apps-managmement',
          pageName: 'AppsManagmement'
        },
        {
          displayName: 'صلاحيات المستخدم',
          icon: 'fa-solid fa-circle',
          route: '/settings/permissions',
          pageName: 'Permissions'
        }
      ]
    }
  ];
  

  getFilteredMenus(): MenuSidebarItem[] {
    return this.menus;
  }

  flattenMenuLevels(menuList: MenuSidebarItem[]): MenuSidebarItem[] {
    return menuList.map(menu => {
      let flatChildren: MenuSidebarItem[] = [];
      if (menu.subMenus && menu.subMenus.length > 0) {
        function collectChildren(items: MenuSidebarItem[]) {
          for (const item of items) {
            const { subMenus, ...rest } = item;
            if (item.route) {
              flatChildren.push({ ...rest, route: item.route, subMenus: [] });
            }

            if (subMenus && subMenus.length > 0) {
              collectChildren(subMenus);
            }
          }
        }
        collectChildren(menu.subMenus);
      }

      return {
        ...menu,
        subMenus: flatChildren.length > 0 ? flatChildren : []
      };
    });
  }
}
