export class GeneralSelectorModel {
    value: any;
    name: any;
    code?: any;
    isSelected?: boolean = false;
    color?: string | null;
    bgColor?: string | null;
    branchId?: number | null;
    branchName?: string | null;
    vacationDays?: number | null;
  }


  export interface CostCenterTreeModel {
    costCenterId: number;
    costCenterNumber: string;
    nameAR: string;
    nameEN: string;
    parentId: number;
    costLevel: number | null;
    isActive: boolean | null;
    isGroup: boolean | null;
    isLocked: boolean | null;
    isParent: boolean | null;
    isPost: boolean | null;
    isExpences: number | null;
    displayOrder: number | null;
    isSelected: boolean;
    isDeleteAction: boolean;
    children: CostCenterTreeModel[];
}