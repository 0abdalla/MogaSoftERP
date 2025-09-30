import { Component, EventEmitter, inject, Input, Output, TemplateRef, ViewChild } from '@angular/core';
import { AccountTreeModel } from '../../../../core/models/settings/accountTree';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { SettingsService } from '../../../../core/services/settings.service';

@Component({
  selector: 'app-fin-tree',
  templateUrl: './fin-tree.component.html',
  styleUrl: './fin-tree.component.css'
})
export class FinTreeComponent {
  private modalService = inject(NgbModal);
  @ViewChild('deleteModal') deleteModal!: TemplateRef<any>;
  @Input() isParentAccount: boolean = false;
  @Input() reloadData: boolean = false;
  @Output() selectedAccount = new EventEmitter<any>();
  selectedAccountId!: number;
  accountTreeData: any[] = [];
  AccountData: any[] = [];
  showLoader: boolean = false;
  showDeleteLoader: boolean = false;
  searchText = '';
  isSearchMode = false;
  parentAccountsList: any[] = [];
  accountTypes: any[] = [];
  currencyType: any[] = [{ currencyId: 1, nameAR: 'جنيه' }, { currencyId: 1, nameAR: 'ريال' }]
  accountTreeModel: AccountTreeModel = {} as AccountTreeModel;

  constructor(private settingService: SettingsService) { }

  ngOnInit(): void {
    this.loadData();
  }

  ngOnChanges(changes:any): void {
    if (changes && changes.reloadData && !changes.reloadData.firstChange) {
      this.loadData();
    }
  }

  selectAccount(account: AccountTreeModel) {
    if (account.isDeleteAction) {
      this.openDeleteModal(account.accountId);
      return
    }
    this.selectedAccount.emit(account);
  }

  loadData() {
    // debugger;
    this.showLoader = true;
    this.settingService.GetAccountTreeHierarchicalData(this.searchText).subscribe(data => {
      this.showLoader = false;
      this.isSearchMode = true;
      this.accountTreeData = data;
    }, (error) => {
      this.showLoader = false;
    }, () => {
      this.showLoader = false;
    });
  }

  openDeleteModal(accountId: number) {
    this.selectedAccountId = accountId;
    this.modalService.open(this.deleteModal, { centered: true, size: 'md' });
  }

  deleteAccount() {
    this.showDeleteLoader = true;
    this.settingService.DeleteAccountTree(this.selectedAccountId).subscribe(data => {

      if (data?.isSuccess) {
        this.modalService?.dismissAll();
        this.loadData();
      }
      else {
      }
      this.showDeleteLoader = false;
    }, err => {
      this.showDeleteLoader = false;
    }, () => {
      this.showDeleteLoader = false;
    });
  }

  dataUpdated(isUpdate: boolean) {
    if (isUpdate) {
      this.loadData();
    }
  }
}
