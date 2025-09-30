import { Component, EventEmitter, Input, Output } from '@angular/core';
import { AccountTreeModel } from '../../../../core/models/settings/accountTree';

@Component({
  selector: 'app-fin-tree-item',
  templateUrl: './fin-tree-item.component.html',
  styleUrl: './fin-tree-item.component.css'
})
export class FinTreeItemComponent {
  
  expanded = false;
  @Input() account!: AccountTreeModel;
  @Output() selectedAccount = new EventEmitter<AccountTreeModel>();
  @Output() dataUpdated = new EventEmitter<boolean>();

  constructor() { }

  ngOnInit(): void {
    this.expanded =
      this.account.isSelected &&
      this.account.children.some((x) => x.isSelected);
  }

  toggleNode(account: AccountTreeModel) {
    this.expanded = !this.expanded;
  }
  
  onEvent(
    e: Event,
    account: AccountTreeModel,
    isDelete: boolean = false
  ): void {
    e.preventDefault();
    e.stopPropagation();
    if (isDelete) {
      account.isDeleteAction = true;
    }
    this.selectAccount(account);
  }

  selectAccount(account: AccountTreeModel) {
    this.selectedAccount.emit(account);
  }

  getLevelClass(level: any) {
    var paddingValue = level;
    if (level > 0) paddingValue = level / 1.1;
    return 'padding-right:' + paddingValue + 'rem !important';
  }
  accountActionUpdated(isUpdate: boolean) {
    this.dataUpdated.emit(isUpdate);
  }
}
