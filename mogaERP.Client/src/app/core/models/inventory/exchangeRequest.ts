export interface ExchangeRequest {
    id: number
    number: string
    date: string
    notes: string
    status: string
    jobDepartmentId: number
    jobDepartmentName: string
    items: Item[]
    createdById: string
    createdBy: string
    createdOn: string
    updatedById: any
    updatedBy: any
    updatedOn: any
  }
  
  export interface Item {
    itemId: number
    itemName: string
    quantity: number
    price: number
    priceAfterTax: number
    unit: string
  }
  