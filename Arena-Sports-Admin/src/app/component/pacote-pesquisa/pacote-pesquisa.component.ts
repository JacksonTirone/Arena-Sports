import { Component, OnInit } from '@angular/core';
import { AguardeService } from '../../service/aguarde.service';
import { ApiService } from '../../service/api.service';
import { MensagemService } from '../../service/mensagem.service';
import { Router } from '@angular/router';
import { ConfirmacaoComponent } from '../confirmacao/confirmacao.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-pacote-pesquisa',
  templateUrl: './pacote-pesquisa.component.html',
  styleUrls: ['./pacote-pesquisa.component.css']
})
export class PacotePesquisaComponent implements OnInit {

  displayedColumns = ["descricao", "itens", "acao"]
  dataSource = []

  constructor(private router: Router, private aguardeService: AguardeService, private service: ApiService, private  mensagem: MensagemService, private dialog: MatDialog) { }

  ngOnInit() {
    this.find();
  }

  find() {
    const aguarde = this.aguardeService.aguarde();

    this.service.getDot('Churrasqueira/GetAllPacote').subscribe(data => {
      if (data.success) {
        this.dataSource = data.data;
        localStorage.setItem('churrasqueiraPacotes', JSON.stringify(data.data));
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
    this.router.navigate(["/pacote-cadastro"]);
  }

  alterar(valor) {
    localStorage.setItem("valor", JSON.stringify(valor));
    this.router.navigate(["/pacote-cadastro/"]);
  }

  openDialogExcluir(selecionado: any): void {
    const dialogRef = this.dialog.open(ConfirmacaoComponent, {
      width: '450px',
      data: {
        titulo: 'Tem certeza?', mensagem: 'Excluir Pacote ' + selecionado.descricao
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      const aguarde = this.aguardeService.aguarde();
      aguarde.close();

      if (result) {
        this.service.excluir('Churrasqueira/DeletePacoteChurrasqueira', selecionado.churrasqueiraPacoteId).subscribe(data => {
          this.mensagem.sucesso("Pacote removido com Sucesso");
          this.find();
        });
      }
    });
  }
}
