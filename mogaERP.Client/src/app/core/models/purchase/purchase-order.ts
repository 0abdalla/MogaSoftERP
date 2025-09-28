export interface PurchaseOrder {
    id: number
    orderNumber: string
    referenceNumber: string
    orderDate: string
    purchaseRequestId: number
    purchaseRequestNumber: string
    supplierId: number
    supplierName: string
    description: string
    status: string
    items: Item[]
  }
  
  export interface Item {
    itemId: number
    itemName: string
    requestedQuantity: number
    quantity: number
    unitPrice: number
  }
  