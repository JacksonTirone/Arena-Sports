import { BrowserModule } from '@angular/platform-browser';
import { LOCALE_ID, NgModule } from '@angular/core';

import localePt from '@angular/common/locales/pt';
registerLocaleData(localePt, 'pt-BR');
import { registerLocaleData } from '@angular/common';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http'; 
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { JwtInterceptor } from './helpers/jwt.interceptor';
import { ErrorInterceptor } from './helpers/error.interceptor';

import { NgxMaskModule, IConfig } from 'ngx-mask'
//export const options: Partial<IConfig> | (() => Partial<IConfig>) = null;

import { CurrencyMaskConfig, CurrencyMaskModule, CURRENCY_MASK_CONFIG } from 'ng2-currency-mask';
export const CustomCurrencyMaskConfig: CurrencyMaskConfig = {
  align: "right",
  allowNegative: true,
  decimal: ",",
  precision: 2,
  prefix: "R$ ",
  suffix: "",
  thousands: "."
};

// *************** FORM CONTROLS ***************
import {MatAutocompleteModule} from '@angular/material/autocomplete';
import {MatCheckboxModule} from '@angular/material/checkbox';
import {MatDatepickerModule} from '@angular/material/datepicker';
import {MatNativeDateModule} from '@angular/material/core';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';
import {MatRadioModule} from '@angular/material/radio';
import {MatSelectModule} from '@angular/material/select';
import {MatSliderModule} from '@angular/material/slider';
import {MatSlideToggleModule} from '@angular/material/slide-toggle';

// *************** NAVIGATION ***************
import {MatMenuModule} from '@angular/material/menu';
import {MatSidenavModule} from '@angular/material/sidenav';
import {MatToolbarModule} from '@angular/material/toolbar';

// *************** LAYOUT ***************
import {MatCardModule} from '@angular/material/card';
import {MatDividerModule} from '@angular/material/divider';
import {MatExpansionModule} from '@angular/material/expansion';
import {MatGridListModule} from '@angular/material/grid-list';
import {MatListModule} from '@angular/material/list';
import {MatStepperModule} from '@angular/material/stepper';
import {MatTabsModule} from '@angular/material/tabs';
import {MatTreeModule} from '@angular/material/tree';

// *************** BUTTONS & INDICATORS ***************
import {MatButtonModule} from '@angular/material/button';
import {MatButtonToggleModule} from '@angular/material/button-toggle';
import {MatBadgeModule} from '@angular/material/badge';
import {MatChipsModule} from '@angular/material/chips';
import {MatIconModule} from '@angular/material/icon';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import {MatProgressBarModule} from '@angular/material/progress-bar';
import {MatRippleModule} from '@angular/material/core';

// *************** POPUPS & MODALS ***************
import {MatBottomSheetModule} from '@angular/material/bottom-sheet';
import {MatDialogModule} from '@angular/material/dialog';
import {MatSnackBarModule} from '@angular/material/snack-bar';
import {MatTooltipModule} from '@angular/material/tooltip';

// *************** DATA TABLE ***************
import {MatPaginatorModule} from '@angular/material/paginator';
import {MatSortModule} from '@angular/material/sort';
import {MatTableModule} from '@angular/material/table';

//Components
import { LoginComponent } from './component/login/login.component';
import { AguardeComponent } from './component/aguarde/aguarde.component';
import { HorarioPesquisaComponent } from './component/horario-pesquisa/horario-pesquisa.component';
import { QuadraCadastroComponent } from './component/quadra-cadastro/quadra-cadastro.component';
import { HomeLayoutComponent } from './component/homeLayout/homeLayout.component';
import { ConfirmacaoComponent } from './component/confirmacao/confirmacao.component';
import { QuadraPesquisaComponent } from './component/quadra-pesquisa/quadra-pesquisa.component';
import { OpcionaisCadastroComponent } from './component/opcionaisQuadra-cadastro/opcionais-cadastro.component';
import { OpcionaisPesquisaComponent } from './component/opcionaisQuadra-pesquisa/opcionais-pesquisa.component';
import { ChurrasqueiraPesquisaComponent } from './component/churrasqueira-pesquisa/churrasqueira-pesquisa.component';
import { ChurrasqueiraCadastroComponent } from './component/churrasqueira-cadastro/churrasqueira-cadastro.component';
import { PacotePesquisaComponent } from './component/pacote-pesquisa/pacote-pesquisa.component';
import { PacoteCadastroComponent } from './component/pacote-cadastro/pacote-cadastro.component';
import { HorarioCadastroComponent } from './component/horario-cadastro/horario-cadastro.component';
import { ReservaCadastroComponent } from './component/reserva-cadastro/reserva-cadastro.component';
import { AdicionarClienteComponent } from './component/adicionar-cliente/adicionar-cliente.component';

const AllMaterialModules=[
  MatAutocompleteModule,
  MatCheckboxModule,
  MatDatepickerModule,
  MatNativeDateModule,
  MatFormFieldModule,
  MatInputModule,
  MatRadioModule,
  MatSelectModule,
  MatSliderModule,
  MatSlideToggleModule,
  MatMenuModule,
  MatSidenavModule,
  MatToolbarModule,
  MatCardModule,
  MatDividerModule,
  MatExpansionModule,
  MatGridListModule,
  MatListModule,
  MatStepperModule,
  MatTabsModule,
  MatTreeModule,
  MatButtonModule,
  MatButtonToggleModule,
  MatBadgeModule,
  MatChipsModule,
  MatIconModule,
  MatProgressSpinnerModule,
  MatProgressBarModule,
  MatRippleModule,
  MatBottomSheetModule,
  MatDialogModule,
  MatSnackBarModule,
  MatTooltipModule,
  MatPaginatorModule,
  MatSortModule,
  MatTableModule,
];

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    HomeLayoutComponent,
    AguardeComponent,
    HorarioPesquisaComponent,
    HorarioCadastroComponent,
    QuadraCadastroComponent,
    QuadraPesquisaComponent,
    ConfirmacaoComponent,
    OpcionaisCadastroComponent,
    OpcionaisPesquisaComponent,
    ChurrasqueiraCadastroComponent,
    ChurrasqueiraPesquisaComponent,
    PacoteCadastroComponent,
    PacotePesquisaComponent,
    ReservaCadastroComponent,
    AdicionarClienteComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    AllMaterialModules,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    ToastrModule.forRoot({
      timeOut: 3000,
      positionClass: 'toast-top-right',
      preventDuplicates: true,
    }),
    CurrencyMaskModule,
    NgxMaskModule.forRoot(),
  ],
  providers: [
    { provide: CURRENCY_MASK_CONFIG, useValue: CustomCurrencyMaskConfig },
    { provide: LOCALE_ID, useValue: 'pt-BR' },
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true }
  ],
  entryComponents:[
    ConfirmacaoComponent,
    ReservaCadastroComponent,
    AguardeComponent,
    AdicionarClienteComponent
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
