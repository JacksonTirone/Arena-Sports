import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { LoginComponent } from './component/login/login.component';
import { AuthGuard } from './auth/auth.guard';
import { HorarioPesquisaComponent } from './component/horario-pesquisa/horario-pesquisa.component';
import { HorarioCadastroComponent } from './component/horario-cadastro/horario-cadastro.component';
import { QuadraCadastroComponent } from './component/quadra-cadastro/quadra-cadastro.component';
import { HomeLayoutComponent } from './component/homeLayout/homeLayout.component';
import { QuadraPesquisaComponent } from './component/quadra-pesquisa/quadra-pesquisa.component';
import { OpcionaisCadastroComponent } from './component/opcionaisQuadra-cadastro/opcionais-cadastro.component';
import { OpcionaisPesquisaComponent } from './component/opcionaisQuadra-pesquisa/opcionais-pesquisa.component';
import { PacotePesquisaComponent } from './component/pacote-pesquisa/pacote-pesquisa.component';
import { PacoteCadastroComponent } from './component/pacote-cadastro/pacote-cadastro.component';
import { ChurrasqueiraCadastroComponent } from './component/churrasqueira-cadastro/churrasqueira-cadastro.component';
import { ChurrasqueiraPesquisaComponent } from './component/churrasqueira-pesquisa/churrasqueira-pesquisa.component';
import { ReservaCadastroComponent } from './component/reserva-cadastro/reserva-cadastro.component';
import { AdicionarClienteComponent } from './component/adicionar-cliente/adicionar-cliente.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: '', component: HomeLayoutComponent, canActivate: [AuthGuard],
    children: [
      { path: '', redirectTo: '/horario-pesquisa', pathMatch: 'full' },
      { path: 'horario-pesquisa', component: HorarioPesquisaComponent },
      { path: 'horario-cadastro', component: HorarioCadastroComponent },
      { path: 'quadra-cadastro', component: QuadraCadastroComponent },
      { path: 'quadra-pesquisa', component: QuadraPesquisaComponent },
      { path: 'opcionais-cadastro', component: OpcionaisCadastroComponent },
      { path: 'opcionais-pesquisa', component: OpcionaisPesquisaComponent },
      { path: 'churrasqueira-cadastro', component: ChurrasqueiraCadastroComponent },
      { path: 'churrasqueira-pesquisa', component: ChurrasqueiraPesquisaComponent },
      { path: 'pacote-cadastro', component: PacoteCadastroComponent },
      { path: 'pacote-pesquisa', component: PacotePesquisaComponent },
      { path: 'reserva-cadastro', component: ReservaCadastroComponent },
      { path: 'adicionar-cliente', component: AdicionarClienteComponent },
    ] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
