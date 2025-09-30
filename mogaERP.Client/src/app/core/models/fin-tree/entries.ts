export interface DailyEntry {
    id: number
    restrictionNumber: string
    restrictionDate: string
    restrictionTypeId: any
    restrictionTypeName: any
    accountingGuidanceId: any
    accountingGuidanceName: any
    description: string
    details: Detail[]
    createdById: any
    createdBy: any
    createdOn: string
    updatedById: any
    updatedBy: any
    updatedOn: any
  }
  
  export interface Detail {
    id: number
    accountId: number
    accountName: string
    debit: number
    credit: number
    costCenterId: any
    costCenterName: any
    note: any
  }
  