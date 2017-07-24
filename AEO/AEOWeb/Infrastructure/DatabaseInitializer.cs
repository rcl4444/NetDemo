using Repository;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using AEOPoco.Domain;
using Repository.Interface;

namespace AEOWeb.Infrastructure
{
    public class DatabaseInitializer : IDataInitializer
    {
        private readonly IEncryptionService _encryptionService;
        private readonly IRepository<CustomsAuthentication> _customsAuthenticationRepository;
        private readonly IRepository<CustomerAccount> _customerAccountRepository;
        private readonly IRepository<FileSchedule> _fileScheduleRepository;
        private readonly IRepository<ScoreTask> _scoreTaskRepository;

        public DatabaseInitializer(IEncryptionService encryptionService,
            IRepository<CustomsAuthentication> customsAuthenticationRepository,
            IRepository<CustomerAccount> customerAccountRepository,
            IRepository<FileSchedule> fileScheduleRepository,
            IRepository<ScoreTask> scoreTaskRepository)
        {
            this._encryptionService = encryptionService;
            this._customsAuthenticationRepository = customsAuthenticationRepository;
            this._customerAccountRepository = customerAccountRepository;
            this._fileScheduleRepository = fileScheduleRepository;
            this._scoreTaskRepository = scoreTaskRepository;
        }

        public void Seed(IDbContext context)
        {
            if (this._customsAuthenticationRepository.TableNoTracking.Any())
            {
                return;
            }
            DateTime createTime = DateTime.Now;
            using (var tran = this._customsAuthenticationRepository.GetUnitOfWork())
            {
                CustomsAuthentication document = new CustomsAuthentication()
                {
                    TitleName = "海关认证企业标准(高级认证)",
                    CreateTime = createTime,
                    OutlineClasses = new List<OutlineClass>()
                {
                    new OutlineClass()
                    {
                        OutlineClassName = "一、内部控制标准",
                        CreateTime = DateTime.Now,
                        Clauseses = new List<Clauses>()
                        {
                            new Clauses()
                            {
                                ClausesName = "(一)组织结构控制",
                                CreateTime = createTime,
                                Items = new List<Item>()
                                {
                                    new Item()
                                    {
                                        ItemName = "1.内部组织架构",
                                        CreateTime = createTime,
                                        FineItems = new List<FineItem>()
                                        {
                                            new FineItem()
                                            {
                                                FineItemName = "(1)进出口业务、财务、内部监督等部门职责分工明确.",
                                                CreateTime = createTime,
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "组织架构图",
                                                        SuggestFileName = "组织架构图.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "各部门职责",
                                                        SuggestFileName = "各部门职责.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            },
                                            new FineItem()
                                            {
                                                FineItemName = "(2).指定高级管理人员负责关务,对企业认证建立书面或者电子档案",
                                                CreateTime = createTime,
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "海关AEO认证项目跟进表和项目档案",
                                                        SuggestFileName = "海关AEO认证项目跟进表和项目档案.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "关于成立AEO认证项目组通知",
                                                        SuggestFileName = "关于成立AEO认证项目组通知.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "法人授权委托书",
                                                        SuggestFileName = "法人授权委托书.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "公司简介",
                                                        SuggestFileName = "公司简介.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new Item()
                                    {
                                        ItemName = "1.海关业务培训",
                                        CreateTime = createTime,
                                        FineItems = new List<FineItem>()
                                        {
                                            new FineItem()
                                            {
                                                FineItemName = "(1)企业应当简历海关法律法规等相关管理规定的内部培训制度.",
                                                CreateTime = createTime,
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "公司培训管理制度",
                                                        SuggestFileName = "公司培训管理制度.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "培训讲义节选（含法律法规）",
                                                        SuggestFileName = "培训讲义节选（含法律法规）.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            },
                                            new FineItem()
                                            {
                                                FineItemName = "(2)法定代表人或者其授权人员、负责关务的高级管理人员应当每年至少参加1次海关法律法规等相关管理规定的内部培训,即使了解、掌握相关管理规定.",
                                                CreateTime = createTime,
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "培训计划表及培训记录表",
                                                        SuggestFileName = "培训计划表及培训记录表.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            },
                            new Clauses()
                            {
                                ClausesName = "(二)进出口业务控制",
                                CreateTime = createTime,
                                Items = new List<Item>()
                                {
                                    new Item()
                                    {
                                        ItemName = "3.单证控制",
                                        CreateTime = createTime,
                                        FineItems = new List<FineItem>()
                                        {
                                            new FineItem()
                                            {
                                                FineItemName = @"具备进出口单证复核或者纠错制度或者程序.
进出口获取收发货人:在申报前或者委托申报前有专门部门或者岗位人员对进出口单证涉及的价格、归类、原产地、数量、品名、规格等内容的真实性、准确性和规范性进行内部复核.
报关企业:代理申报前,有专门部门或者岗位人员对委托人提供的监管证件、商业单据、进出口单证等资料的真实性、完整性和有效性进行合理审查.
物流企业:有专门部门或者岗位人员对运输工具进出境申报信息、舱单及相关电子数据、转关单(载货清单)等物流信息的准确性、一致性进行复核.",
                                                CreateTime = createTime,
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "进出口单证复核/纠错制度/程序",
                                                        SuggestFileName = "进出口单证复核/纠错制度/程序.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "报关单异常记录表",
                                                        SuggestFileName = "报关单异常记录表.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new Item()
                                    {
                                        ItemName = "4.单证保管",
                                        CreateTime = createTime,
                                        FineItems = new List<FineItem>()
                                        {
                                            new FineItem()
                                            {
                                                FineItemName = "(1)按海关要求建立进出口单证管理制度,确保企业保存的进出口纸质和电子报关单证、物流信息档案的及时性、完整性、准确性与安全性.",
                                                CreateTime = createTime,
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "进出口单证管理制度",
                                                        SuggestFileName = "进出口单证管理制度.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "抽查单证10份（不足10份全查）",
                                                        SuggestFileName = "抽查单证10份（不足10份全查）.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            },
                                            new FineItem()
                                            {
                                                FineItemName = "(2)妥善保管报关专用印章,以及海关核发的证书、法律文书.",
                                                CreateTime = createTime,
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "印章管理办法",
                                                        SuggestFileName = "印章管理办法.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "印章及证书图片",
                                                        SuggestFileName = "印章及证书图片.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "用章登记表及用章审批表",
                                                        SuggestFileName = "用章登记表及用章审批表.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "公司资质证书及借领登记表",
                                                        SuggestFileName = "公司资质证书及借领登记表.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new Item()
                                    {
                                        ItemName = "4.进出口活动",
                                        CreateTime = createTime,
                                        FineItems = new List<FineItem>()
                                        {
                                            new FineItem()
                                            {
                                                FineItemName = "进出口业务管理流程设置合理、完备,涉及的货物流、单证流、信息流能够得到有效控制,经抽查,未发现有不符合海关监管规定的情形.",
                                                CreateTime = createTime,
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "进出口管理规章制度",
                                                        SuggestFileName = "进出口管理规章制度.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "货物申报和管理的规定",
                                                        SuggestFileName = "货物申报和管理的规定.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "存货盘点表",
                                                        SuggestFileName = "存货盘点表.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "报关截图",
                                                        SuggestFileName = "报关截图.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "操作流程图",
                                                        SuggestFileName = "操作流程图.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            },
                            new Clauses()
                            {
                                ClausesName = "(三)内部审计控制",
                                CreateTime = createTime,
                                Items = new List<Item>()
                                {
                                    new Item()
                                    {
                                        ItemName = "6.内审制度",
                                        CreateTime = createTime,
                                        FineItems = new List<FineItem>()
                                        {
                                            new FineItem()
                                            {
                                                FineItemName = "(1)设立专门的内部审计机构或者岗位,或者聘请外部专职人员独立对进出口业务等实施内部审计.",
                                                CreateTime = createTime,
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "《内部审计管理制度》",
                                                        SuggestFileName = "《内部审计管理制度》.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "财务内审报告",
                                                        SuggestFileName = "财务内审报告.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "行政内审报告",
                                                        SuggestFileName = "行政内审报告.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "商务内审报告",
                                                        SuggestFileName = "商务内审报告.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "进出口专项审计报告",
                                                        SuggestFileName = "进出口专项审计报告.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "内部审计工作手册汇编",
                                                        SuggestFileName = "内部审计工作手册汇编.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            },
                                            new FineItem()
                                            {
                                                FineItemName = "(2)每年至少内审1次,建立内审书面或者电子档案.",
                                                CreateTime = createTime,
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "《内部审计管理制度》",
                                                        SuggestFileName = "《内部审计管理制度》.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "财务内审报告",
                                                        SuggestFileName = "财务内审报告.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "行政内审报告",
                                                        SuggestFileName = "行政内审报告.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "商务内审报告",
                                                        SuggestFileName = "商务内审报告.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "进出口专项审计报告",
                                                        SuggestFileName = "进出口专项审计报告.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "内部审计工作手册汇编",
                                                        SuggestFileName = "内部审计工作手册汇编.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new Item()
                                    {
                                        ItemName = "7.责任追究",
                                        CreateTime = createTime,
                                        FineItems = new List<FineItem>()
                                        {
                                            new FineItem()
                                            {
                                                FineItemName = "(1)建立对进出口业务发现问题或者违法行为的责任追究制度或者措施.",
                                                CreateTime = createTime,
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "责任追究管理制度（违规行为）",
                                                        SuggestFileName = "责任追究管理制度（违规行为）.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "责任追究记录表（有需提供）",
                                                        SuggestFileName = "责任追究记录表（有需提供）.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            },
                                            new FineItem()
                                            {
                                                FineItemName = "(2)建立对企业人员和报关人员私揽货物报关、假借海关名义牟利、向海关人员行贿等行为的责任追究制度或者措施.",
                                                CreateTime = createTime,
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "责任追究管理制度（违规行为）",
                                                        SuggestFileName = "责任追究管理制度（违规行为）.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "责任追究记录表（有需提供）",
                                                        SuggestFileName = "责任追究记录表（有需提供）.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new Item()
                                    {
                                        ItemName = "8.改进机制",
                                        CreateTime = createTime,
                                        FineItems = new List<FineItem>()
                                        {
                                            new FineItem()
                                            {
                                                FineItemName = "(1)建立改进制度或者措施.",
                                                CreateTime = createTime,
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "改进制度和流程控制",
                                                        SuggestFileName = "改进制度和流程控制.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            },
                                            new FineItem()
                                            {
                                                FineItemName = "(2)对海关要求的规范改进事项,应由负责关务的高级管理人员直接负责具体规范改进实施.",
                                                CreateTime = createTime,
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "整改事项会议记录",
                                                        SuggestFileName = "整改事项会议记录.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "整改报告",
                                                        SuggestFileName = "整改报告.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "整改跟进表",
                                                        SuggestFileName = "整改跟进表.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "抽查记录表",
                                                        SuggestFileName = "抽查记录表.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            },
                            new Clauses()
                            {
                                ClausesName = "(四)信息系统控制",
                                CreateTime = createTime,
                                Items = new List<Item>()
                                {
                                    new Item()
                                    {
                                        ItemName = "9.信息系统",
                                        CreateTime = createTime,
                                        FineItems = new List<FineItem>()
                                        {
                                            new FineItem()
                                            {
                                                FineItemName = "具备真是、准确、完整、有效记录企业生产经营、进出口或者代理报关活动的信息系统,特别是财务控制、关务、物流控制等功能模块有效运行.",
                                                CreateTime = createTime,
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "财务、关务等系统操作手册",
                                                        SuggestFileName = "财务、关务等系统操作手册.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new Item()
                                    {
                                        ItemName = "10.数据管理",
                                        CreateTime = createTime,
                                        FineItems = new List<FineItem>()
                                        {
                                            new FineItem()
                                            {
                                                FineItemName = "(1)生产经营数据以及与进出口活动有关的数据及时、准确、完整录入系统.系统自进出口货物办结海关手续之日起保存3年以上.",
                                                CreateTime = createTime,
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "系统数据界面（及时性、准确性、完整性）",
                                                        SuggestFileName = "系统数据界面（及时性、准确性、完整性）.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            },
                                            new FineItem()
                                            {
                                                FineItemName = "(2)进出口或者代理报关活动等主要环节在系统中能够实现流程检索、跟踪..",
                                                CreateTime = createTime,
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "系统数据界面（报关环节）",
                                                        SuggestFileName = "系统数据界面（报关环节）.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new Item()
                                    {
                                        ItemName = "11.信息安全",
                                        CreateTime = createTime,
                                        FineItems = new List<FineItem>()
                                        {
                                            new FineItem()
                                            {
                                                FineItemName = "(1)建立信息安全管理制度,保护信息系统安全,并对员工进行相关培训.",
                                                CreateTime = createTime,
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "信息安全管理制度",
                                                        SuggestFileName = "信息安全管理制度.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "信息安全培训及课件",
                                                        SuggestFileName = "信息安全培训及课件.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            },
                                            new FineItem()
                                            {
                                                FineItemName = @"(2)有专门程序或者制度,识别信息系统的非正常使用,包括非法入侵信息系统,篡改或者更改业务数据,并对上述行为有严格的责任追究.信息系统要使用专人账户和密码,并且定期更改用户密码.",
                                                CreateTime = createTime,
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "服务器机房图片",
                                                        SuggestFileName = "服务器机房图片.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "信息系统账户、密码管理，定期更改截图",
                                                        SuggestFileName = "信息系统账户、密码管理，定期更改截图.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            },
                                            new FineItem()
                                            {
                                                FineItemName = @"(3)有专门程序或者制度,保护系统和数据,有数据恢复、备份等手段防止信息丢失,应用反病毒软件和防火墙技术.",
                                                CreateTime = createTime,
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "信息系统备份记录",
                                                        SuggestFileName = "信息系统备份记录.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "备份电脑界面",
                                                        SuggestFileName = "备份电脑界面.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "反病毒软件截图",
                                                        SuggestFileName = "反病毒软件截图.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "防火墙软件截图",
                                                        SuggestFileName = "防火墙软件截图.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    },
                    new OutlineClass()
                    {
                        OutlineClassName = "二、财务状况标准",
                        CreateTime = DateTime.Now,
                        Clauseses = new List<Clauses>()
                        {
                            new Clauses()
                            {
                                ClausesName = "(五)财务状况",
                                CreateTime = createTime,
                                Items = new List<Item>()
                                {
                                    new Item()
                                    {
                                        ItemName = "12.会计信息",
                                        CreateTime = createTime,
                                        FineItems = new List<FineItem>()
                                        {
                                            new FineItem()
                                            {
                                                FineItemName = "(1)会计账账簿和财务会计报告等会计资料真实、准确、完整记录和反映进出口活动的有关情况,财务处理及时、规范.",
                                                CreateTime = createTime,
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "财务审计报告",
                                                        SuggestFileName = "财务审计报告.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            },
                                            new FineItem()
                                            {
                                                FineItemName = "(2)企业申请认证的,提交当年度会计师事务所审计报告,审计报告所反映的企业财务状况真实、完整、规范、合法;重新认证的,企业自称为高级认证企业起每年接受会计师事务所所审计,审计报告所反映的企业财务状况真实、完整、规范、合法.",
                                                CreateTime = createTime,
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "财务审计报告",
                                                        SuggestFileName = "财务审计报告.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new Item()
                                    {
                                        ItemName = "13.偿付能力",
                                        CreateTime = createTime,
                                        FineItems = new List<FineItem>()
                                        {
                                            new FineItem()
                                            {
                                                FineItemName = "(1)企业财务的速动比率在安全或者正常范围内.",
                                                CreateTime = createTime,
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "财务指标计算表（固定资产和单笔那谁最高额）",
                                                        SuggestFileName = "财务指标计算表（固定资产和单笔那谁最高额）.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "保函（财务指标有不达标的）",
                                                        SuggestFileName = "保函（财务指标有不达标的）.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            },
                                            new FineItem()
                                            {
                                                FineItemName = "(2)企业财务的资产负债率在安全或者正常范围内.",
                                                CreateTime = createTime,
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "财务指标计算表（固定资产和单笔那谁最高额）",
                                                        SuggestFileName = "财务指标计算表（固定资产和单笔那谁最高额）.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "保函（财务指标有不达标的）",
                                                        SuggestFileName = "保函（财务指标有不达标的）.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new Item()
                                    {
                                        ItemName = "14.盈利能力",
                                        CreateTime = createTime,
                                        FineItems = new List<FineItem>()
                                        {
                                            new FineItem()
                                            {
                                                FineItemName = "企业主营业务利润率在安全或者正常范围内.",
                                                CreateTime = createTime,
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "财务指标计算表（固定资产和单笔那谁最高额）",
                                                        SuggestFileName = "财务指标计算表（固定资产和单笔那谁最高额）.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "保函（财务指标有不达标的）",
                                                        SuggestFileName = "保函（财务指标有不达标的）.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new Item()
                                    {
                                        ItemName = "15.缴税能力",
                                        CreateTime = createTime,
                                        FineItems = new List<FineItem>()
                                        {
                                            new FineItem()
                                            {
                                                FineItemName = @"生产型进出口货物收发货人:上月末固定资产净值不低于其3年内向海关单笔纳税最高额;
非生产型进出口货物收发货人:上年度经营性现金净流量不为负.",
                                                CreateTime = createTime,
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "财务指标计算表（固定资产和单笔那谁最高额）",
                                                        SuggestFileName = "财务指标计算表（固定资产和单笔那谁最高额）.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "保函（财务指标有不达标的）",
                                                        SuggestFileName = "保函（财务指标有不达标的）.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    },
                    new OutlineClass()
                    {
                        CreateTime = createTime,
                        OutlineClassName = "三、守法规范标准",
                        Clauseses = new List<Clauses>()
                        {
                            new Clauses()
                            {
                                CreateTime = createTime,
                                ClausesName = "(六)遵守法律法规",
                                Items = new List<Item>()
                                {
                                    new Item()
                                    {
                                        CreateTime = createTime,
                                        ItemName = "16.人员违法",
                                        FineItems = new List<FineItem>()
                                        {
                                            new FineItem()
                                            {
                                                CreateTime = createTime,
                                                FineItemName = "企业法定代表人(负责人)、负责关务的高级管理人员和财务负责人连续2年无故意犯罪记录.",
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "（法人）无犯罪证明",
                                                        SuggestFileName = "（法人）无犯罪证明.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "（关务负责人）无犯罪证明",
                                                        SuggestFileName = "（关务负责人）无犯罪证明.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "（财务经理）无犯罪证明",
                                                        SuggestFileName = "（财务经理）无犯罪证明.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "（商务经理、报关）无犯罪证明",
                                                        SuggestFileName = "（商务经理、报关）无犯罪证明.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new Item()
                                    {
                                        CreateTime = createTime,
                                        ItemName = "17.企业违法",
                                        FineItems = new List<FineItem>()
                                        {
                                            new FineItem()
                                            {
                                                CreateTime = createTime,
                                                FineItemName = "(1)连续2年无走私犯罪、走私行为.",
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "公司申明",
                                                        SuggestFileName = "公司申明.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "AEO证书（原本为AEO企业的需提供）",
                                                        SuggestFileName = "AEO证书（原本为AEO企业的需提供）.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "税务违法记录证明",
                                                        SuggestFileName = "税务违法记录证明.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "工商信用网违法记录查询截图",
                                                        SuggestFileName = "工商信用网违法记录查询截图.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            },
                                            new FineItem()
                                            {
                                                CreateTime = createTime,
                                                FineItemName = @"(2)非报关企业:连续1年无因违反海关监管规定被处罚金额超过3万年的行为;
报关企业:连续1年无因违法海关监管规定被处罚金额超过1万年的行为.",
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "公司申明",
                                                        SuggestFileName = "公司申明.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "AEO证书（原本为AEO企业的需提供）",
                                                        SuggestFileName = "AEO证书（原本为AEO企业的需提供）.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "税务违法记录证明",
                                                        SuggestFileName = "税务违法记录证明.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "工商信用网违法记录查询截图",
                                                        SuggestFileName = "工商信用网违法记录查询截图.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            },
                                            new FineItem()
                                            {
                                                CreateTime = createTime,
                                                FineItemName = @"(3)非报关企业:1年内违反海关监管规定行为的处罚金额累计5万元以下,且违法次数在5次以下或者虽然超过5次,但违法次数与上年度企业进出口相关单证(报关单及进出境备案单清单、运输工具进出境申报信息、舱单及相关电子数据、转关单(载货清单))总票数比例不超过千分之一(企业自查发现并主动向海关报明,被海关处以警告以及3万元以下罚款的除外)
报关企业:1年内违反海关监管规定行为的次数不超过上年度代理申报报关单及进出境备案清单总票数的万分之一,且处罚金额累计3万元以下.(企业自查发现并主动向海关报明,被海关处以警告以及1万元以下罚款的除外)",
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "公司申明",
                                                        SuggestFileName = "公司申明.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "AEO证书（原本为AEO企业的需提供）",
                                                        SuggestFileName = "AEO证书（原本为AEO企业的需提供）.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "税务违法记录证明",
                                                        SuggestFileName = "税务违法记录证明.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "工商信用网违法记录查询截图",
                                                        SuggestFileName = "工商信用网违法记录查询截图.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            }
                                        }
                                    }

                                }
                            },
                            new Clauses()
                            {
                                CreateTime = createTime,
                                ClausesName = "(七)进出口业务规范",
                                Items = new List<Item>()
                                {
                                    new Item()
                                    {
                                        CreateTime = createTime,
                                        ItemName = "18.注册信息",
                                        FineItems = new List<FineItem>()
                                        {
                                            new FineItem()
                                            {
                                                CreateTime = createTime,
                                                FineItemName = @"报关单位:按规定报送《报关单位注册信息年度报告》,企业及报关人员在海关的注册登记内容与实际相符.
其他企业:在海关的注册登记内容与实际相符.",
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "营业执照、税务登记证、公司章程",
                                                        SuggestFileName = "营业执照、税务登记证、公司章程.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "海关注册登记证书",
                                                        SuggestFileName = "海关注册登记证书.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "企业经营管理报告",
                                                        SuggestFileName = "企业经营管理报告.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "公司资质图片",
                                                        SuggestFileName = "公司资质图片.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "《单位注册信息年度报告》",
                                                        SuggestFileName = "《单位注册信息年度报告》.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new Item()
                                    {
                                        CreateTime = createTime,
                                        ItemName = "19.进出口记录",
                                        FineItems = new List<FineItem>()
                                        {
                                            new FineItem()
                                            {
                                                CreateTime = createTime,
                                                FineItemName = "上年度或者本年度有进出口活动或者为进出口活动提供相关服务.",
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "进出口记录",
                                                        SuggestFileName = "进出口记录.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new Item()
                                    {
                                        CreateTime = createTime,
                                        ItemName = "20.申报(传输)规范",
                                        FineItems = new List<FineItem>()
                                        {
                                            new FineItem()
                                            {
                                                CreateTime = createTime,
                                                FineItemName = @"(1)报关企业:连续4个季度单季报关差错率不超过同期全国平均报关差错率.
进出口货物收发货人:连续4个季度单季报关差错率或者所委托报关企业报关差错率不超过同期全国平均报关差错率.
物流企业:连续4个季度单季舱单以及相关电子数据传输差错率不超过同期全国平均传输差错率,连续4个季度单季运输工具进出境申报信息、转关单(载货清单)等物流信息的申报差错率不超过同期全国平均申报差错率.",
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "改单记录表",
                                                        SuggestFileName = "改单记录表.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            },
                                            new FineItem()
                                            {
                                                CreateTime = createTime,
                                                FineItemName = "(2)连续2个季度单机规范申报率超过90%.",
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "改单记录表",
                                                        SuggestFileName = "改单记录表.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            },
                                            new FineItem()
                                            {
                                                CreateTime = createTime,
                                                FineItemName = "(3)上季度及本年1至上月手(账)册超期未报核情事不超过1次.",
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "改单记录表",
                                                        SuggestFileName = "改单记录表.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    new Item()
                                    {
                                        CreateTime = createTime,
                                        ItemName = "21.税款缴纳",
                                        FineItems = new List<FineItem>()
                                        {
                                            new FineItem()
                                            {
                                                CreateTime = createTime,
                                                FineItemName = "(1)上年度以及本年度1至上月滞纳税款报关单率不超过5%.",
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "纳税证明（国税）",
                                                        SuggestFileName = "纳税证明（地税）.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "纳税证明（国税）",
                                                        SuggestFileName = "纳税证明（地税）.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            },
                                            new FineItem()
                                            {
                                                CreateTime = createTime,
                                                FineItemName = "(2)截至认证期间,没有超过法定缴款期限尚未缴纳税款及罚没款项情事.",
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "纳税证明（国税）",
                                                        SuggestFileName = "纳税证明（地税）.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "纳税证明（国税）",
                                                        SuggestFileName = "纳税证明（地税）.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            },
                            new Clauses()
                            {
                                CreateTime = createTime,
                                ClausesName = "(八)符合海关管理要求",
                                Items = new List<Item>
                                {
                                    new Item()
                                    {
                                        CreateTime = createTime,
                                        ItemName = "22.管理要求",
                                        FineItems = new List<FineItem>()
                                        {
                                            new FineItem()
                                            {
                                                CreateTime = createTime,
                                                FineItemName = "(1)连续2年未发现有向海关提供虚假情况或者隐瞒重要事实、拒绝或者拖延提供账簿单证资料、故意转移、隐匿、篡改、毁弃账簿单证资料等逃避海关稽查、逃避税款征缴的情形,或者无正当理由拒不配合海关执法或者管理的情形.",
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "公司申明",
                                                        SuggestFileName = "公共信息中心证明界面.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "公司申明",
                                                        SuggestFileName = "公共信息中心证明界面.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            },
                                            new FineItem()
                                            {
                                                CreateTime = createTime,
                                                FineItemName = "(2)连续2年未发现企业报送信息有隐瞒真实情况、弄虚作假的情形.",
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "公司申明",
                                                        SuggestFileName = "公共信息中心证明界面.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "公司申明",
                                                        SuggestFileName = "公共信息中心证明界面.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            },
                                            new FineItem()
                                            {
                                                CreateTime = createTime,
                                                FineItemName = "(3)连续2年未发现有假借海关或者其他企业名义获取不当利益的情形.",
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "公司申明",
                                                        SuggestFileName = "公共信息中心证明界面.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "公司申明",
                                                        SuggestFileName = "公共信息中心证明界面.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            },
                                            new FineItem()
                                            {
                                                CreateTime = createTime,
                                                FineItemName = "(4)连续2年未发现有向海关人员行贿的行为.",
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "公司申明",
                                                        SuggestFileName = "公共信息中心证明界面.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "公司申明",
                                                        SuggestFileName = "公共信息中心证明界面.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            },
                            new Clauses()
                            {
                                 CreateTime = createTime,
                                 ClausesName = "(九)未有外部信用",
                                 Items = new List<Item>()
                                    {
                                        new Item()
                                        {
                                            CreateTime = createTime,
                                            ItemName = "23.外部信用",
                                            FineItems = new List<FineItem>()
                                            {
                                                new FineItem()
                                                {
                                                    CreateTime = createTime,
                                                    FineItemName = "企业或者其他法定代表人(负责人)、负责关务的高级管理人员、财务负责人连续1年在工商、商务、税务、银行、外汇、检验检疫、公安、检察院、法院等部门未被列入经营异常名录、失信企业或者人员名单、黑名单企业、人员.",
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "（法人）个人信用",
                                                        SuggestFileName = "（法人）个人信用.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "（财务副总）个人信用",
                                                        SuggestFileName = "（财务副总）个人信用.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "（关务负责人）个人信用",
                                                        SuggestFileName = "（关务负责人）个人信用.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "银行企业信用报告",
                                                        SuggestFileName = "银行企业信用报告.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "工商信用网截图",
                                                        SuggestFileName = "工商信用网截图.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "企业自我申明",
                                                        SuggestFileName = "企业自我申明.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                                }
                                            }
                                        }
                                    }
                            }
                        }
                    },
                    new OutlineClass()
                    {
                        CreateTime = createTime,
                        OutlineClassName = "四、贸易安全标准",
                        Clauseses = new List<Clauses>()
                        {
                            new Clauses()
                            {
                                CreateTime = createTime,
                                ClausesName = "(十)场所安全控制措施",
                                Items = new List<Item>()
                                {
                                    new Item()
                                    {
                                        CreateTime = createTime,
                                        ItemName = "24.场所安全",
                                        FineItems = new List<FineItem>()
                                        {
                                            new FineItem()
                                            {
                                                CreateTime = createTime,
                                                FineItemName = @"企业有检查、阻止未载明的货物和未经许可的人员进入场所、货物装载和储存区域的书面制度和程序;进出口货物进出的区域设有隔离设施,以防止未经许可的人员进入.
(1)大门和传达室:车辆、人员进出的大门配备人员驻守.
(2)建筑结构:建筑物的建造方式能够防止非法闯入.定期对建筑物进行检查和修缮.确保其完好无损.
(3)照明:企业生产经营场所配备充足的照明,包括以下区域:出入口,货物装载和储存区,围墙周边及停车场/停车区域.
(4)报警系统及视频监控摄像机:装配报警系统和视频监控摄像机,检测以下区域:出入口,货物装卸和储存区,围墙周边及停车场/停车区域,防止未经许可进入货物存储以及装卸区.
(5)存储区域:在货物装卸和储存区域,以及用于存放进出口货物的区域,设有隔离设施,以及阻止任何未经许可的人员进入.
(6)锁闭装置及钥匙保管:所有内外窗户,大门和围栏都设有足够数量的锁闭装置.管理层或者保安人员要保管所有锁和钥匙.",
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "仓库管理制度",
                                                        SuggestFileName = "仓库管理制度.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "仓库周边图片",
                                                        SuggestFileName = "仓库周边图片.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "库内巡查记录表",
                                                        SuggestFileName = "库内巡查记录表.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "仓库照明视频图片",
                                                        SuggestFileName = "仓库照明视频图片.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "红外安防说明书",
                                                        SuggestFileName = "红外安防说明书.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "锁匙月核查记录表",
                                                        SuggestFileName = "锁匙月核查记录表.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            },
                            new Clauses()
                            {
                                CreateTime = createTime,
                                ClausesName = "(十一)进入安全控制措施",
                                Items = new List<Item>()
                                {
                                    new Item()
                                    {
                                        CreateTime = createTime,
                                        ItemName = "25.进入安全",
                                        FineItems = new List<FineItem>()
                                        {
                                            new FineItem()
                                            {
                                                CreateTime = createTime,
                                                FineItemName = @"企业实行门禁管理,有实施员工和访客进出、保护公司资产的书面制度和程序.
(1)员工:具有员工身份识别系统,对员工进行身份识别和进入控制.对员工、访客的身份标识(比如钥匙、钥匙卡等)的发放和回收进行统一管理和登记.
(2)访客:对进入企业的访客要检查带有照片的身份证件并进行登记,访客要佩戴练市身份标识并且有内部人员陪同.
(3)未经许可进入、身份不明的人员:有识别、质询和确认未经许可进入、身份不明的人员的程序;发现可疑人员进入的,企业员工要及时报告.",
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "外来人员来访管理制度",
                                                        SuggestFileName = "外来人员来访管理制度.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "库内外来加工人员管理规定",
                                                        SuggestFileName = "库内外来加工人员管理规定.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "来访登记表",
                                                        SuggestFileName = "来访登记表.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "行车记录表",
                                                        SuggestFileName = "行车记录表.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            },
                            new Clauses()
                            {
                                CreateTime = createTime,
                                ClausesName = "(十二)人员安全控制措施",
                                Items = new List<Item>()
                                {
                                    new Item()
                                    {
                                        CreateTime = createTime,
                                        ItemName = "26.人员安全",
                                        FineItems = new List<FineItem>()
                                        {
                                            new FineItem()
                                            {
                                                CreateTime = createTime,
                                                FineItemName = @"企业有审查拟聘员工和定期审查现有员工的书面制度和程序,提供动态员工清单,包含姓名、出生日期、身份证号码、担任职位.
(1)聘用前审核:聘用员工前,要对其应聘申请信息(例如就业经历、推荐信等)进行核实.
(2)背景调查:聘用员工前,要对其进行有无违法犯罪记录等安全背景的检查或者调查.一经录用,要根据员工的表现,以及对处于重要敏感工作岗位的员工进行定期审查和重新调查.
(3)员工离职程序:有书面制度和程序,对离职或者停职员工及时收回工作证件、设备,并禁止其进入企业生产经营场所及使用企业信息系统.
(4)安全培训:要对员工进行供应链安全意识的日常培训,员工要了解企业应对某种状况以及进行报告的程序.",
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "员工手册",
                                                        SuggestFileName = "员工手册.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "人事名录清单",
                                                        SuggestFileName = "人事名录清单.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "招聘、入职、离职流程",
                                                        SuggestFileName = "招聘、入职、离职流程.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "个别员工入职及离职资料",
                                                        SuggestFileName = "个别员工入职及离职资料.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "员工调查表",
                                                        SuggestFileName = "员工调查表.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "员工无犯罪记录证明",
                                                        SuggestFileName = "员工无犯罪记录证明.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            },
                            new Clauses()
                            {
                                CreateTime = createTime,
                                ClausesName = "(十三)商业伙伴安全控制措施",
                                Items = new List<Item>()
                                {
                                    new Item()
                                    {
                                        CreateTime = createTime,
                                        ItemName = "27.商业伙伴安全",
                                        FineItems = new List<FineItem>()
                                        {
                                            new FineItem()
                                            {
                                                CreateTime = createTime,
                                                FineItemName = @"企业有评估、要求、检查商业伙伴供应链安全的书面制度和程序.
(1)全面评估:在筛选商业伙伴时根据本认证标准对商业伙伴进行全面评估,重点评估守法合规和贸易安全,并有书面制度和程序.
(2)书面文件:在合同、协议或者其他书面资料中要求商业伙伴按照认证标准优化和完善贸易安全管理.
(3)监控检查:定期监控或者检查商业伙伴遵守贸易安全要求的情况,并有书面制度和程序.",
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "供货商合作伙伴选择与安全管理程序",
                                                        SuggestFileName = "供货商合作伙伴选择与安全管理程序.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "香港港联控股公司合作合同及资料",
                                                        SuggestFileName = "香港港联控股公司合作合同及资料.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "能健恒合作合同及AEO证书",
                                                        SuggestFileName = "能健恒合作合同及AEO证书.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "港联协议及评估表",
                                                        SuggestFileName = "港联协议及评估表.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            },
                            new Clauses()
                            {
                                CreateTime = createTime,
                                ClausesName = "(十四)货物安全控制措施",
                                Items = new List<Item>()
                                {
                                    new Item()
                                    {
                                        CreateTime = createTime,
                                        ItemName = "28.货物安全",
                                        FineItems = new List<FineItem>()
                                        {
                                            new FineItem()
                                            {
                                                CreateTime = createTime,
                                                FineItemName = @"企业有确保供应链中货物在运输、搬运和存放过程中的完整性和安全性的措施和程序.
(1)装运和接收货物:运抵的货物要与货物单证的信息相符,核实货物的重量、标签、件数或者箱数.离岸的货物要与购货订单或者装运订单上的内容进行核实.在货物关键交接环节有签名、盖章等保护制度.
(2)货物差异:在出现货物溢、短装或者其他异常现象时要及时报告或者采取其他应对措施,并有书面制度和程序.",
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "仓库管理程序",
                                                        SuggestFileName = "仓库管理程序.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "仓管员操作规范",
                                                        SuggestFileName = "仓管员操作规范.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "库内货物管理规定",
                                                        SuggestFileName = "库内货物管理规定.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "存库盘点表",
                                                        SuggestFileName = "存库盘点表.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            },
                            new Clauses()
                            {
                                CreateTime = createTime,
                                ClausesName = "(十五)集装箱安全控制措施",
                                Items = new List<Item>()
                                {
                                    new Item()
                                    {
                                        CreateTime = createTime,
                                        ItemName = "29.集装箱安全",
                                        FineItems = new List<FineItem>()
                                        {
                                            new FineItem()
                                            {
                                                CreateTime = createTime,
                                                FineItemName = @"企业有确保集装箱完整性,以防止未经许可的货物或者人员混入的措施和程序.
(1)集装箱检查:在装货前检查集装箱结构的物理完整性和可靠性,包括门的锁闭系统的可靠性,并做好相关登记.检查建议采取“七点检查法”(即对集装箱按照以下顺序检查:前壁、左侧、右侧、地板、顶部、内/外门、外部/起落架).
(2)集装箱封条:已装货集装箱要施加高安全的封条,所有封条都要符合或者超出现行 PAS ISO 17712 对高度安全封条的标准,封条有专人管理、登记.要建立施加和检验封条的书面制度和程序,以及封条异常的报告机制.
(3)集装箱存储:集装箱要保存在安全区域,以防止未经许可的进入或者改装,有报告和解决未经许可擅自进入集装箱或者集装箱存储区域的程序.",
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "货物出仓操作规定",
                                                        SuggestFileName = "叉车司机操作规定.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "货物入仓操作规定",
                                                        SuggestFileName = "货物入仓操作规定.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "封条管理规定",
                                                        SuggestFileName = "封条管理规定.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            },
                            new Clauses()
                            {
                                CreateTime = createTime,
                                ClausesName = "(十六)运输工具安全控制措施",
                                Items = new List<Item>()
                                {
                                    new Item()
                                    {
                                        CreateTime = createTime,
                                        ItemName = "30.运输工具安全",
                                        FineItems = new List<FineItem>()
                                        {
                                            new FineItem()
                                            {
                                                CreateTime = createTime,
                                                FineItemName = @"企业有确保运输工具(拖车和挂车)的完整性,防止未经许可的人员或者物品混入的书面制度和程序.
(1)运输工具的检查程序:有专门程序或者制度检查出入运输工具,防止藏匿可疑物品.
(2)运输工具存储:运输工具要停放在安全区域,以防止未经许可的进入或者其他损害,有报告和解决未经许可擅自进入或者损害的程序.
(3)司机身份核实:在货物被接受或者发放前,应对装运或者接收货物的驾驶员进行身份认定.",
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "叉车司机操作规定",
                                                        SuggestFileName = "叉车司机操作规定.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "放行条管理规定",
                                                        SuggestFileName = "放行条管理规定.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "检查车头记录",
                                                        SuggestFileName = "检查车头记录.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            },
                            new Clauses()
                            {
                                CreateTime = createTime,
                                ClausesName = "(十七)危机管理控制措施",
                                Items = new List<Item>()
                                {
                                    new Item()
                                    {
                                        CreateTime = createTime,
                                        ItemName = "31.危机管理",
                                        FineItems = new List<FineItem>()
                                        {
                                            new FineItem()
                                            {
                                                CreateTime = createTime,
                                                FineItemName = @"企业有应对灾害或者紧急安全事故等异常情况的书面制度和程序.
(1)应对机制:具备对灾害或者紧急安全事故等异常情况的报告、处置等应急程序或者机制.
(2)应急培训:要对员工进行应急培训.
(3)异常报告:发现有灾害或者紧急安全事故等异常情况、非法或者可疑活动,要报告海关或者其他有关执法机关.",
                                                FileRequires = new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "安全领导小组",
                                                        SuggestFileName = "安全领导小组.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "消防安全管理程序",
                                                        SuggestFileName = "消防安全管理程序.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "消防报警设备管理规定",
                                                        SuggestFileName = "消防报警设备管理规定.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "消防设备安全操作规程",
                                                        SuggestFileName = "消防设备安全操作规程.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "消防栓使用与保养规定",
                                                        SuggestFileName = "消防栓使用与保养规定.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "公司消防安全管理规定",
                                                        SuggestFileName = "公司消防安全管理规定.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "公司危机管理制度",
                                                        SuggestFileName = "公司危机管理制度.docx",
                                                        CreateTime = createTime
                                                    },
                                                    new FileRequire()
                                                    {
                                                        Description = "自然灾害突发事件应急预案",
                                                        SuggestFileName = "自然灾害突发事件应急预案.docx",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    },
                    new  OutlineClass()
                    {
                        CreateTime = createTime,
                        OutlineClassName = "五、附加标准",
                        Clauseses = new List<Clauses>()
                        {
                            new Clauses()
                            {
                                CreateTime = createTime,
                                ClausesName = "(十八)加分标准",
                                Items = new List<Item>
                                {
                                    new Item()
                                    {
                                        CreateTime = createTime,
                                        ItemName = "32.加分项目",
                                        FineItems = new List<FineItem>()
                                        {
                                            new FineItem()
                                            {
                                                CreateTime = createTime,
                                                FineItemName = @"有下列情形之一的,经海关确认后可以加分:
(1)属于海关特殊监管区域内企业.
(2)属于国家鼓励和扶持的信息技术、节能环保、新能源、高端装备制造、新材料等产业之一的企业.
(3)被中国报关协会等全国性行业组织评为优秀报关企业等荣誉称号的.
(4)属于中国外贸出口先导指数样本企业,且1年内填报问卷及时率在90%以上、问卷答案与出口增速的吻合度在0.3以上的;或者属于进口货物使用去向调查样本企业、其他统计专项调查样本企业,且一年内填报问卷及时率和复核准确率在90%以上的.
(5)属于积极配合海关开展报关单证企业存单,且连续4个季度单季存单及时率、准确率高达全国平均水平的企业.",
                                                FileRequires =new List<FileRequire>()
                                                {
                                                    new FileRequire()
                                                    {
                                                        Description = "test",
                                                        SuggestFileName="test",
                                                        CreateTime = createTime
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                };
                foreach (var outlineClasse in document.OutlineClasses)
                {
                    outlineClasse.CustomsAuthentication = document;
                    foreach (var clausese in outlineClasse.Clauseses)
                    {
                        clausese.OutlineClass = outlineClasse;
                        foreach (var item in clausese.Items)
                        {
                            item.Clauses = clausese;
                            foreach (var fineItem in item.FineItems)
                            {
                                fineItem.Item = item;
                                foreach (var filerequire in fineItem.FileRequires)
                                {
                                    filerequire.FineItem = fineItem;
                                }
                            }
                        }
                    }
                }
                //this._customsAuthenticationRepository.Insert(document);
                CustomerCompany company = new CustomerCompany()
                {
                    CompanyName = "测试公司",
                    UniqueFlag = "testcompany",
                    //CustomsAuthentication = document,
                    CreateTime = createTime,
                    CustomerDeparements = new List<CustomerDeparement>()
                {
                    new CustomerDeparement()
                    {
                        DeparementName = "业务部",
                        CreateTime = createTime
                    }
                }
                };
                foreach (var deparement in company.CustomerDeparements)
                {
                    deparement.CustomerCompany = company;
                }
                CustomerAccount admin = new CustomerAccount()
                {
                    AccountName = "admin",
                    PassWord = _encryptionService.CreatePasswordDefault("123456"),
                    PersonName = "王大棒",
                    IsManager = true,
                    CreateTime = createTime,
                    CustomerCompany = company
                };
                this._customerAccountRepository.Insert(admin);
                CustomerAccount user1 = new CustomerAccount()
                {
                    AccountName = "person1",
                    PassWord = _encryptionService.CreatePasswordDefault("123456"),
                    PersonName = "人一",
                    IsManager = false,
                    CreateTime = createTime,
                    CustomerCompany = company,
                    CustomerDeparement = company.CustomerDeparements.First()
                };
                this._customerAccountRepository.Insert(user1);
                CustomerAccount user2 = new CustomerAccount()
                {
                    AccountName = "person2",
                    PassWord = _encryptionService.CreatePasswordDefault("123456"),
                    PersonName = "人二",
                    IsManager = false,
                    CreateTime = createTime,
                    CustomerCompany = company,
                    CustomerDeparement = company.CustomerDeparements.First()
                };
                this._customerAccountRepository.Insert(user2);
                CustomerAccount user3 = new CustomerAccount()
                {
                    AccountName = "person3",
                    PassWord = _encryptionService.CreatePasswordDefault("123456"),
                    PersonName = "人三",
                    IsManager = false,
                    CreateTime = createTime,
                    CustomerCompany = company,
                    CustomerDeparement = company.CustomerDeparements.First()
                };
                this._customerAccountRepository.Insert(user2);
                FileSchedule fs1 = new FileSchedule()
                {
                    FinishTime = DateTime.Now.AddDays(-1),
                    ChargePerson = user1,
                    Auditor = user2,
                    CreateTime = createTime,
                    FileRequire = document.OutlineClasses.FirstOrDefault().Clauseses.FirstOrDefault().Items.FirstOrDefault().FineItems.FirstOrDefault().FileRequires.FirstOrDefault()
                };
                //this._fileScheduleRepository.Insert(fs1);
                FileSchedule fs2 = new FileSchedule()
                {
                    FinishTime = DateTime.Now.AddDays(-1),
                    ChargePerson = user1,
                    Auditor = user2,
                    CreateTime = createTime,
                    FileRequire = document.OutlineClasses.FirstOrDefault().Clauseses.FirstOrDefault().Items.FirstOrDefault().FineItems.FirstOrDefault().FileRequires.Skip(1).First()
                };
                //this._fileScheduleRepository.Insert(fs2);
                ScoreTask st = new ScoreTask()
                {
                    Item = document.OutlineClasses.FirstOrDefault().Clauseses.FirstOrDefault().Items.FirstOrDefault(),
                    ScorePerson = user1,
                    CreateTime = createTime,
                    CustomerCompany = company
                };
                //this._scoreTaskRepository.Insert(st);
                tran.Commit();
            }
        }
    }
}