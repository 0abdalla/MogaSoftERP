export interface debitNotification {
    id: number
    date: string
    bankId: number
    bankName: string
    accountId: number
    accountName: string
    checkNumber: string
    amount: number
    notes: string
    dailyRestriction: DailyRestriction
    createdById: string
    createdBy: string
    createdOn: string
    updatedById: string
    updatedBy: string
    updatedOn: string
  }
  
  export interface DailyRestriction {
    id: number
    restrictionNumber: string
    restrictionDate: string
    accountingGuidanceName: any
    from: string
    to: string
    amount: number
    number: any
  }
  