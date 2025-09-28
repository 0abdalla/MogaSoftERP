export interface PriceQuotation {
    id: number
    quotationNumber: string
    quotationDate: string
    supplierId: number
    supplierName: string
    notes: string
    status: string
    totalAmount: number
    purchaseRequestId: number
    purchaseRequestNumber: string
    items: Item[]
    createdById: any
    createdBy: any
    createdOn: string
    updatedById: any
    updatedBy: any
    updatedOn: any
  }
  
  export interface Item {
    itemId: number
    name: string
    quantity: number
    unitPrice: number
    unit: string
    notes: string
    totalPrice: number
  }
  