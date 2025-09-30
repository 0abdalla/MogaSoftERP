export interface ExchangePermission {
    id: number
    permissionNumber: string
    documentNumber: any
    permissionDate: string
    storeId: number
    storeName: string
    jobDepartmentId: number
    jobDepartmentName: string
    notes: string
    items: Item[]
    disbursementRequestId: number
    disbursementRequestNumber: string
    dailyRestriction: DailyRestriction
  }
  
  export interface Item {
    itemId: number
    itemName: string
    unit: string
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
  