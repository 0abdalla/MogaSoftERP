import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  loignForm!: FormGroup;
  forgetPassword!:FormGroup;
  message:string = '';
  forgetMessage:string = '';
  constructor(private router : Router , private fb : FormBuilder , private authService : AuthService) { } 
  ngOnInit(): void { 
    this.loignForm = this.fb.group({
      userName: ['' , Validators.required],
      password: ['' , Validators.required]
    })
    this.forgetPassword = this.fb.group({
      userName: ['' , Validators.required],
    })
  }
  onLogin(){
    this.authService.login(this.loignForm.value).subscribe({
      next:(data:any) => {
        this.router.navigate(['/dashboard']);
        
      },
      error:(err) => {
        this.message = 'المستخدم أو كلمة المرور غير صحيحين';
        console.log(err);
        
      }
    })
  }
  onForgetPassword() {
    // if (this.forgetPassword.invalid) {
    //   this.forgetPassword.markAllAsTouched();
    //   return;
    // }
  
    // this.authService.forgetPassword(this.forgetPassword.value).subscribe({
    //   next: (data: any) => {
    //     Swal.fire({
    //       title: 'تم إرسال رابط إعادة التعيين إلى البريد الإلكتروني المسجل',
    //       icon: 'success',
    //       showConfirmButton: false,
    //       timer: 2000
    //     })
    //     setTimeout(() => {
    //       const modalElement = document.getElementById('forgetPasswordModal');
    //       if (modalElement) {
    //         const modalInstance = bootstrap.Modal.getInstance(modalElement);
    //         modalInstance?.hide();
    //       }
    //     }, 1000);
    //   },
    //   error: (err) => {
    //     Swal.fire({
    //       title: 'المستخدم غير صحيح',
    //       icon: 'error',
    //       showConfirmButton: false,
    //       timer: 2000
    //     })
    //   }
    // });
  }
  
  showPassword = false;

  togglePassword() {
    this.showPassword = !this.showPassword;
  }
}
