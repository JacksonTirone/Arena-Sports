import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from  '@angular/forms';
import { Router } from  '@angular/router';
import { MensagemService } from 'src/app/service/mensagem.service';
import { AuthService } from '../../auth/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  hide = true;
  loading = false;
  error = '';
  loginForm: FormGroup;

  get formControls() { return this.loginForm.controls; }
  get emailInput() { return this.loginForm.get('email'); }
  get passwordInput() { return this.loginForm.get('password'); }  

  constructor(private authService: AuthService, private router: Router) { }

  getErrorMessage() {
    return this.loginForm.hasError('email') ? 'E-mail inválido' : '';
  }

  ngOnInit(): void {
    this.loginForm = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', [Validators.required ])
    });

    this.authService.logout();
  }

  login() {
    if (this.loginForm.invalid) {
      return;
    }
    
    this.loading = true;
    this.authService.login(this.loginForm.value)
      .subscribe(
        data => {
          localStorage.setItem('token', '' + data.data.token);
          localStorage.setItem('nome', data.data.usuarioLogado.nome);
          this.authService.CarregarDefaults();
          this.router.navigate(["/"]);
        },
        error => {
          this.error = error;
          this.loading = false;
        });
  }
}
