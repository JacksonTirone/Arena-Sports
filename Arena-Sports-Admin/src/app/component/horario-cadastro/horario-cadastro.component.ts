import { Component, OnInit } from '@angular/core';
import { AguardeService } from '../../service/aguarde.service';
import { ApiService } from '../../service/api.service';
import { MensagemService } from '../../service/mensagem.service';
import { Observable, of } from 'rxjs';
import { ConfirmacaoComponent } from '../confirmacao/confirmacao.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-horario-cadastro',
  templateUrl: './horario-cadastro.component.html',
  styleUrls: ['./horario-cadastro.component.css']
})
export class HorarioCadastroComponent implements OnInit {

  acao: string
  data = { quadraId: 0, diaSemana: 0, duracao: 0, valor: 0 };
  constructor(private aguardeService: AguardeService, private service: ApiService, private mensagem: MensagemService, private dialog: MatDialog) { }

  clicado = false;
  
  quadras = []
  horarios =[]
  horasDuracao = ['00', '01', '02', '03']
  horas = ['01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12', '13', '14', '15', '16', '17', '18', '19', '20', '21', '22', '23', '00']
  minutos = ['00', '15', '30', '45']
  horaInicio = "00"
  horaFim = "00"
  minutoInicio = "00"
  minutoFim = "00"
  minutosDuracao = "00"
  horaDuracao = "00"

  ngOnInit() {
    this.acao = "Cadastrar"
    this.findQuadras();
  }

  findQuadras() {
    const aguarde = this.aguardeService.aguarde();

    this.service.getDot('Quadra/GetAll').subscribe(data => {
      if (data.success) {
        this.quadras = data.data;
      } else {
        this.mensagem.erro(data.message);
      }
      aguarde.close();
    }, error => {
      this.mensagem.erro("Não foi possível executar a ação: " + error);
      aguarde.close();
    });
  }

  salvar() {
    this.verificaErros().subscribe(erro => {
      if (!erro) {
        const aguarde = this.aguardeService.aguarde();

        let envelope = {
          quadraId: this.data.quadraId,
          diaSemana: Number(this.data.diaSemana),
          timeInicio: this.horaInicio + ":" + this.minutoInicio,
          timeFim: this.horaFim + ":" + this.minutoFim,
          duracao: this.horaDuracao + ":" + this.minutosDuracao,
          valor: Number(this.data.valor)
        }
        this.service.postRequestDefault('Horario/ConfigurarHorarioQuadra', envelope).subscribe(data => {
          if (data.success) {
            aguarde.close();
            this.mensagem.sucesso("Horario configurado com Sucesso");
            this.findHorarios(this.data.quadraId,this.data.diaSemana);
          } else {
            this.mensagem.erro(data.message);
            aguarde.close();
          }
        }, error => {
          this.mensagem.erro(error);
          aguarde.close();
        });
      }
    });

  }

  vazioClicado(valor) {
    if (this.clicado) {
      return this.vazioNumero(valor);
    }
  }

  onChangeQuadra(event){
    this.data.diaSemana = null;
    this.horarios = [];
  }

  onChangeDia(event){
    this.findHorarios(this.data.quadraId,event.value);
  }

  findHorarios(quadra, dia) {
    const aguarde = this.aguardeService.aguarde();
    let envelope = {quadraId: quadra,  diaSemana: dia};
    this.service.postRequestDefault('Horario/GetConfiguracaoHorarios', envelope).subscribe(data => {
      if (data.success) {
        this.horarios = data.data;
      } else {
        this.mensagem.erro(data.message);
      }
      aguarde.close();
    }, error => {
      this.mensagem.erro("Não foi possível executar a ação: " + error);
      aguarde.close();
    });
  }

  openDialogExcluir(selecionado: any): void {
    const dialogRef = this.dialog.open(ConfirmacaoComponent, {
      width: '450px',
      data: {
        titulo: 'Tem certeza?', mensagem: 'Excluir o horário ' + selecionado.timeInicio + ' as ' + selecionado.timeFim
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      const aguarde = this.aguardeService.aguarde();
      aguarde.close();

      if (result) {
        this.service.excluir('Horario/DeleteConfiguracaoHorario', selecionado.quadraConfiguracaoHorarioId).subscribe(data => {
          this.mensagem.sucesso("Horario removido com Sucesso");     
          this.findHorarios(this.data.quadraId,this.data.diaSemana);     
        });
      }
    });
  }

  verificaErros(): Observable<boolean> {
    this.clicado = true;
    let erro = false;

    if (this.vazioNumero(this.data.quadraId)) {
      erro = true;
    }
    if (this.vazioNumero(this.data.valor)) {
      erro = true;
    }
    if (this.vazioNumero(this.data.diaSemana)) {
      erro = true;
    }
    return of(erro);
  }

  vazioNumero(valor: any){
    return (valor === undefined || valor === 0);
  }
}
