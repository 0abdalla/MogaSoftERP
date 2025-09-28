export interface PurchaseRequest {
    id: number
    requestNumber: string
    requestDate: string
    dueDate: string
    purpose: string
    storeId: number
    storeName: string
    status: string
    notes: string
    items: Item[]
  }
  
  export interface Item {
    id: number
    itemId: number
    itemName: string
    quantity: number
    notes: string
  }
  