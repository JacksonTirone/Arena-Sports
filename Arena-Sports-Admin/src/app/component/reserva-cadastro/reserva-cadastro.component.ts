import { Component, OnInit, Inject } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { MatCheckboxChange } from '@angular/material/checkbox';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Observable } from 'rxjs';
// import { empty } from "rxjs/observable/empty";
import { of, EMPTY } from 'rxjs';
import { startWith } from 'rxjs/internal/operators/startWith';
import { debounceTime, finalize, map, switchMap, tap } from 'rxjs/operators';
import { Cliente } from 'src/app/model/cliente';
import { AguardeService } from 'src/app/service/aguarde.service';
import { ApiService } from 'src/app/service/api.service';
import { MensagemService } from 'src/app/service/mensagem.service';
import { MatDialog } from '@angular/material/dialog';
import { AdicionarClienteComponent } from '../adicionar-cliente/adicionar-cliente.component';
import { MatRadioChange } from '@angular/material/radio';

@Component({
  selector: 'app-reserva-cadastro',
  templateUrl: './reserva-cadastro.component.html',
  styleUrls: ['./reserva-cadastro.component.css']
})

export class ReservaCadastroComponent {

  pacotes = [];
  churrasqueiras = [];
  opcionais = [];
  searchClientesCtrl = new FormControl('', [Validators.required]);
  filteredOptions: Observable<Cliente[]>;
  total = 0;
  totalOpcionais = 0;
  totalChurrasqueira = 0;

  constructor(
    public dialogRef: MatDialogRef<ReservaCadastroComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any, private aguardeService: AguardeService, private service: ApiService, private mensagem: MensagemService, private dialog: MatDialog) {

    this.data.quadraItemOpcionalIds = [];
    this.data.churrasqueiraId = 0;
    this.data.churrasqueiraPacoteId = 0;
    this.filteredOptions = this.searchClientesCtrl.valueChanges
      .pipe(
        startWith(''),
        debounceTime(800),
        switchMap(value => value.length >= 2 ? this.service.getClientesTop10(value) : new Observable<Cliente[]>())
      );
    this.load();
  }

  displayFn(cliente: Cliente): string {
    return cliente && cliente.nome ? cliente.nome : '';
  }

  load() {
    this.pacotes = JSON.parse(localStorage.getItem("churrasqueiraPacotes"));
    this.opcionais = JSON.parse(localStorage.getItem("quadraOpcionais"));
    this.findChurrasqueiras();
  }
  
  calcularTotal() {
    this.total = this.data.valor + this.totalOpcionais + this.totalChurrasqueira;
    return this.total;
  }
  
  changePacote(event: MatRadioChange) {
    this.totalChurrasqueira = event.value.valor;
  }

  addCliente() {
    const aguarde = this.aguardeService.aguarde();
    const dialogRef = this.dialog.open(AdicionarClienteComponent, {
      width: '600px',
      data: {
        titulo: 'Adicionar Cliente', 
        mensagem: ''
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.searchClientesCtrl.setValue(result);
      }
      aguarde.close();
    });
  }

  findChurrasqueiras() {
    let envelope = { "DataReserva": this.data.dataReserva, "TimeInicio": this.data.timeInicio };
    this.service.postRequestDefault('Churrasqueira/GetAllDisponiveis', envelope).subscribe(data => {
      if (data.success) {
        this.churrasqueiras = data.data;
        this.churrasqueiras.unshift({ churrasqueiraId: 0, descricao: 'Sem Churrasqueira'});
      } else {
        this.mensagem.erro(data.message);
      }
    }, error => {
      this.mensagem.erro("Não foi possível executar a ação: " + error);
    });
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  changeChurras(churrasId) {
    if (churrasId == 0) {
      this.data.churrasqueiraPacoteId = undefined;
      this.totalChurrasqueira = 0;
    }
  }

  marcar(event: MatCheckboxChange, quadraItemOpcional) {
    if (event.checked) {
      this.data.quadraItemOpcionalIds.push(quadraItemOpcional.quadraItemOpcionalId);
      this.totalOpcionais += quadraItemOpcional.valor;
    } else {
      this.data.quadraItemOpcionalIds.splice(this.data.quadraItemOpcionalIds.indexOf(quadraItemOpcional.quadraItemOpcionalId), 1);
      this.totalOpcionais -= quadraItemOpcional.valor;
    }
  }

  onBlurMethod(){
   if (this.searchClientesCtrl.value.usuarioId === undefined){
      this.searchClientesCtrl.setErrors( { 'incorrect': true } );
    };
  }

  reservar(): void {
    if (!this.data.editar && this.searchClientesCtrl.invalid) {
      return;
    }
    if (this.data.churrasqueiraId !== 0 && this.data.churrasqueiraPacoteId === 0) {
      return;
    }
    const aguarde = this.aguardeService.aguarde();
    let envelope = {
      quadraConfiguracaoHorarioId: this.data.quadraConfiguracaoHorarioId,
      quadraId: this.data.quadraId,
      dataReserva: this.data.dataReserva,
      timeInicio: this.data.timeInicio,
      churrasqueiraId: this.data.churrasqueiraId,
      churrasqueiraPacoteId: this.data.churrasqueiraPacoteId.churrasqueiraPacoteId,
      quadraItemOpcionalIds: this.data.quadraItemOpcionalIds,
      usuarioId: this.searchClientesCtrl.value.usuarioId
    }
    this.service.postRequestDefault('Reserva/RealizarReserva', envelope).subscribe(data => {
      if (data.success) {
        this.mensagem.sucesso("Quadra reservada!");
        this.dialogRef.close();
      } else {
        this.mensagem.erro(data.message);
      }
      aguarde.close();
    }, error => {
      this.mensagem.erro(error);
      aguarde.close();
    });
  }
}
