import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AccountTreeModel } from '../../../../core/models/settings/accountTree';
import { GeneralSelectorModel } from '../../../../core/models/settings/costCenter';
import { SettingsService } from '../../../../core/services/settings.service';

@Component({
  selector: 'app-fin-tree-edit-item',
  templateUrl: './fin-tree-edit-item.component.html',
  styleUrl: './fin-tree-edit-item.component.css'
})
export class FinTreeEditItemComponent {
  @Input() isUpdate: boolean = false;
  @Input() groupAccountId!: number;
  @Input() accountModel: AccountTreeModel = {} as AccountTreeModel;
  @Output() dataUpdated = new EventEmitter<boolean>();
  showLoader: boolean = false;
  parentAccountsList: any[] = [];
  accountTypesSelectorData: GeneralSelectorModel[] = [
    { name: 'مدين', value: 1 },
    { name: 'دائن', value: 2 },
  ];
  parentAccountsSelectorData: GeneralSelectorModel[] = [];
  selectedAccountId!: number;
  public formGroup!: FormGroup;

  constructor(private modalService: NgbModal, private form: FormBuilder, private settingService: SettingsService) { }

  ngOnInit(): void { }

  openNewSidePanel(content: any) {
    this.loadSelectors();
    this.initNewForm();
    this.formGroup.patchValue({ parentAccountId: this.groupAccountId });
    this.modalService.open(content, { centered: true, size: 'lg', fullscreen: 'lg' });
  }


  initNewForm() {
    this.buildForm();
    if (this.isUpdate)
      this.fillEditForm(this.accountModel);
    else {
      this.accountModel = {} as AccountTreeModel;
      if (this.groupAccountId)
        this.generateAccountNumber(this.groupAccountId);
      else
        this.generateAccountNumber();
    }

  }
  buildForm() {
    this.formGroup = this.form.group({
      accountId: [null],
      accountNumber: [null],
      parentAccountId: [null],
      accountTypeId: [null, [Validators.required]],
      nameAR: [null, [Validators.required]],
      costCenterId: [null],
      isActive: [true, [Validators.required]],
      isGroup: [false, [Validators.required]],
      isReadOnly: [false, [Validators.required]],
      notes: [null],
    });

    this.formGroup.get('parentAccountId')?.valueChanges.subscribe((parentAccountId: number) => {
      if (!this.isUpdate) {
        this.generateAccountNumber(parentAccountId ? parentAccountId : 0);
      }
    });
  }

  saveAccount() {
    this.accountModel = this.formGroup.value;
    if (this.accountModel?.accountId)
      this.editAccount();
    else
      this.addNewAccount();
  }

  addNewAccount() {
    this.showLoader = true;
    this.settingService.AddNewAccount(this.accountModel).subscribe(data => {
      // debugger;
      if (data?.isSuccess) {
        this.initNewForm();
        this.modalService?.dismissAll();
        this.dataUpdated.emit(true);
      }
      else {
      }
      this.showLoader = false;
    }, err => {
      this.showLoader = false;
    }, () => {
      this.showLoader = false;
    });
  }

  editAccount() {
    this.showLoader = true;
    this.settingService.EditAccountTree(this.accountModel.accountId, this.accountModel).subscribe(data => {
      if (data?.isSuccess) {
        this.isUpdate = false;
        this.modalService?.dismissAll();
        this.initNewForm();
        this.dataUpdated.emit(true);
      }
      else {
      }
      this.showLoader = false;
    }, err => {
      this.showLoader = false;
    }, () => {
      this.showLoader = false;
    });


  }

  generateAccountNumber(parentAccountId: number = 0) {
    this.settingService.GenerateAccountNumber(parentAccountId).subscribe(data => {
      if (data) {
        this.formGroup?.patchValue({ accountNumber: data });
      }
    }, err => {

    }, () => {

    });
  }


  fillEditForm(accountModel: AccountTreeModel) {
    this.isUpdate = true;
    this.formGroup.patchValue({
      accountId: accountModel.accountId,
      accountNumber: accountModel.accountNumber,
      accountTypeId: accountModel.accountTypeId,
      parentAccountId: accountModel.parentAccountId,
      nameAR: accountModel.nameAR,
      nameEN: accountModel.nameAR,
      costCenterId: accountModel.costCenterId,
      isActive: accountModel.isActive,
      isGroup: accountModel.isGroup,
      isReadOnly: accountModel.isReadOnly,
    });
  }

  loadSelectors() {
    this.settingService.GetAccountsSelector(true).subscribe(data => {
      this.parentAccountsSelectorData = data;
    });
  }

  getSelectedParentAccount(account: AccountTreeModel) {
    this.accountModel.parentAccountId = account.accountId;
    this.accountModel.accountLevel = account.accountLevel ? account.accountLevel + 1 : 1;
  }
}
