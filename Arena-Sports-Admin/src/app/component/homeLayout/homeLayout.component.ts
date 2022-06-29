import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/auth/auth.service';

@Component({
  selector: 'app-homeLayout',
  templateUrl: './homeLayout.component.html',
  styleUrls: ['./homeLayout.component.css']
})
export class HomeLayoutComponent implements OnInit {

  constructor(private authenticationService: AuthService) { }

  nome: string;

  ngOnInit(): void {
    this.nome = localStorage.getItem("nome");
  }

  logout() {
    this.authenticationService.logout();
  }
}
