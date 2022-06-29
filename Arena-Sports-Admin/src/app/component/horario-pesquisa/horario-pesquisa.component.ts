import { Component, OnInit } from '@angular/core';
import { AguardeService } from '../../service/aguarde.service';
import { ApiService } from '../../service/api.service';
import { MensagemService } from '../../service/mensagem.service';
import { MatDialog } from '@angular/material/dialog';
import { ReservaCadastroComponent } from '../reserva-cadastro/reserva-cadastro.component';
import * as moment from 'moment-timezone';
import { ConfirmacaoComponent } from '../confirmacao/confirmacao.component';

@Component({
  selector: 'app-horario-pesquisa',
  templateUrl: './horario-pesquisa.component.html',
  styleUrls: ['./horario-pesquisa.component.css']
})
export class HorarioPesquisaComponent implements OnInit {

  dataSource = [];
  selecionados = [];
  minDate =  moment.tz('america/sao_paulo').format();
  data = moment.tz('america/sao_paulo').format();

  constructor(private aguardeService: AguardeService, private service: ApiService, private  mensagem: MensagemService, private dialog: MatDialog) { }

  ngOnInit() {
    this.find();
  }

  find() {
    const aguarde = this.aguardeService.aguarde();

    this.service.postRequestDefault('Horario/GetHorariosQuadras', { data: this.data })
      .subscribe(data => {
          if (data.success) {
            this.dataSource = data.data;
          } else {
            this.mensagem.erro(data.message);
          }
          aguarde.close();
        }, error => {
          this.mensagem.erro("Não foi possível executar a ação: " + error);
          aguarde.close();
        });
  }

  openReserva(dataReserva, quadra, selecionado: any, editar: boolean): void {
    const dialogRef = this.dialog.open(ReservaCadastroComponent, {
      width: '600px',
      data: {
        quadraConfiguracaoHorarioId: selecionado.quadraConfiguracaoHorarioId,
        titulo: 'Reservar ' + quadra, 
        mensagem: selecionado.timeInicio + ' às ' + selecionado.timeFim,
        quadraId: selecionado.quadraId,
        timeInicio: selecionado.timeInicio,
        dataReserva: dataReserva,
        valor: selecionado.valor,
        reserva: selecionado.reserva,
        editar: editar
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      this.find();
    });
  }

  onChange(event){
    this.data = event.value;
    this.find();
  }

  getEsporte(esporteId) {
    if (esporteId === 1) {
      return "Futebol Society 7";
    }
    if (esporteId === 2) {
      return "Futebol Society 5";
    }
    if (esporteId === 3) {
      return "Futevolei";
    }
  }

  openDialogCancelar(selecionado: any): void {
    const dialogRef = this.dialog.open(ConfirmacaoComponent, {
      width: '450px',
      data: {
        titulo: 'Tem certeza?', mensagem: 'Cacelar Reserva de ' + selecionado.nomeCliente
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      const aguarde = this.aguardeService.aguarde();
      aguarde.close();

      if (result) {
        this.service.excluir('Reserva/CancelarReserva',  selecionado.reservaId).subscribe(data => {
          this.mensagem.sucesso("Reserva cancelada com sucesso");
          this.find();
        });
      }
    });
  }
}
