import { Component, OnInit } from '@angular/core';
import { AguardeService } from '../../service/aguarde.service';
import { ApiService } from '../../service/api.service';
import { MensagemService } from '../../service/mensagem.service';
import { Router } from '@angular/router';
import { ConfirmacaoComponent } from '../confirmacao/confirmacao.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-quadra-pesquisa',
  templateUrl: './quadra-pesquisa.component.html',
  styleUrls: ['./quadra-pesquisa.component.css']
})
export class QuadraPesquisaComponent implements OnInit {

  displayedColumns = ["descricao", "piso","esporte","coberta","acao"]
  dataSource = []

  constructor(private router: Router, private aguardeService: AguardeService , private service: ApiService, private  mensagem: MensagemService, private dialog: MatDialog) { }

  ngOnInit() {
    this.find() 
  }

  find() {
    const aguarde = this.aguardeService.aguarde();

    this.service.getDot('Quadra/GetAll').subscribe(data => {
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

  cadastrar() {
    this.router.navigate(["/quadra-cadastro"]);
  }

  getPiso (pisoId){    
    if (pisoId === 1) {
      return 'Sintetico';
    }
    if (pisoId === 2) {
      return 'Areia';
    }
  }

  getEsporte(esporteId) {    
    if (esporteId === 1) {
      return 'FutebolSociety7';
    }
    if (esporteId === 2) {
      return 'FutebolSociety5';
    }
    if (esporteId === 3) {
      return 'Futevolei';
    }
  }

  alterar(valor) {
    localStorage.setItem("valor", JSON.stringify(valor));
    this.router.navigate(["/quadra-cadastro/"]);
  }

  openDialogExcluir(selecionado: any): void {
    const dialogRef = this.dialog.open(ConfirmacaoComponent, {
      width: '450px',
      data: {
        titulo: 'Tem certeza?', mensagem: 'Excluir Quadra ' + selecionado.descricao
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      const aguarde = this.aguardeService.aguarde();
      aguarde.close();

      if (result) {
        this.service.excluir('Quadra/DeleteQuadra', selecionado.quadraId).subscribe(data => {
          this.mensagem.sucesso("Quadra removido com Sucesso");
          this.find();
        });
      }
    });
  }
}
