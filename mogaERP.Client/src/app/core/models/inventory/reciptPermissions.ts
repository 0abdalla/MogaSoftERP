export interface ReciptPermission {
    id: number
    permissionNumber: string
    documentNumber: string
    permissionDate: string
    notes: string
    supplierId: number
    supplierName: string
    storeId: number
    storeName: string
    status: string
    items: Item[]
    purchaseOrderId: number
    purchaseOrderNumber: string
    dailyRestriction: DailyRestriction
  }
  
  export interface Item {
    itemId: number
    itemName: string
    unit: string
    unitId: number
    quantity: number
    unitPrice: number
    totalPrice: number
  }
  
  export interface DailyRestriction {
    id: any
    restrictionNumber: any
    restrictionDate: string
    accountingGuidanceName: any
    from: any
    to: any
    amount: number
    number: any
  }
  export interface PagedResult<T> {
    data: T[];
    totalCount: number;
  }
  