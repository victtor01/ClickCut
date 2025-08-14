# Sistema de agendamento

## Fluxo

**`Usuário` faz autenticação e terá opção de se conectar a uma `loja`** 

**A `loja` por sua vez poderá ter vários `Agendamentos` onde cada `Agendamento` terá os seguintes dados abaixo:**

```csharp
public class Loja {
	public string Nome { get; set; }
	public string Dono { get; set; }
	public List<User> Membros { get ; set; }
	public Password? Password { get; set; } = null;
	public List<Agendamento> Agendas { get; set; }
}
```

**As lojas por sua vez, há agendas**

```csharp
public class Agendamento {
	public string Id { get; set; }
	public string Nome { get; set; }
	public Cliente? Cliente { get; set; }
	public Status Status { get; set; } = Status.CREATED;
	public DateTime startAt { get; set; }
	public DateTime endAt { get; set; }
}
```

**Com seu status:**
```csharp
public enum Status {
  CREATED,    // Agendamento criado, mas ainda não processado.
  PENDING,    // Aguardando confirmação (opcional).
  CONFIRMED,  // Agendamento confirmado e pronto para acontecer
  ACTIVATED,  // Agendamento em andamento/ativo.
  COMPLETED,  // Agendamento concluído (mas ainda não pago).
  PAID,       // Agendamento concluído e pago.
  CANCELLED,  // Agendamento cancelado pelo cliente ou pela loja.
  NO_SHOW,    // Cliente não compareceu.
}
```

**Que por sua vez, pode ter um cliente**


```csharp
public class Cliente {
	public string? Nome { get; set; }
	public string? NumeroDeTelefone { get; set; }
	public List<Agenda> Agendas { get; set; }
}
```
**Além disso podemos criar `Serviços`**

```csharp
public class Servico {
  public string Id { get; set; }
  public string Nome { get; set; } // Ex: "Corte Masculino", "Coloração"
  public string Descricao { get; set; } // Opcional, ex: "Corte com máquina e tesoura, inclui lavagem."
  public decimal Preco { get; set; } // Valor do serviço
  public int DuracaoEmMinutos { get; set; } // Essencial para calcular o endAt do agendamento
  public string FotoUrl { get; set; } // URL de uma imagem do serviço
  public bool Ativo { get; set; } = true; // Permite desativar um serviço sem excluí-lo
}

```

## Features

### `/auth`
**Endpoint para autenticação, o usuário pode ser autenticar.**
